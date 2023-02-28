using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// код данного босса держится на крови и костях, просьба отнестись с понимаем
public class DarkKingHead : Enemy
{
    [Header("Init")]
    [SerializeField] [Tooltip("Attacks will spawn at this area")]  private Collider2D battleZone;
    [SerializeField] private Sword raw;
    [SerializeField] private Sword falldamageDiler;

    [SerializeField] private State pacmanAttackState;
    [SerializeField] private WaitingState waveAttackState;
    [SerializeField] private FlyToTargetState flyState;
    [SerializeField] private FlyToTargetState fallState;
    [SerializeField] private GameObject[] spheres;
    [SerializeField] private Bow waveSpawner;
    [SerializeField] private GameObject portalPrefab;

    [Header("Balance")]
    [SerializeField] private float timeBetweenWaveAttacks = 1.5f;
    [SerializeField] private int jumpAttacks=3;
    [SerializeField] private int portalsNum = 5;
    [SerializeField] private float portalsSpawnMaxDif = 3f;
    
    private float targetYPosition = 0f;
    private float waveSpawnReload = 0f;
    private int jumpAttacksLeft;
    private List<GameObject> spawnedPortals;
    
    private enum AttackType{Null, Pacman, Spheres, Waves, Jump, }

    private AttackType currentAttack;
    private Animator _animator;
    
    private void Awake()
    {
        base.Awake();
        spawnedPortals = new List<GameObject>();
        targetYPosition = transform.position.y;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Vector2 pos = transform.position;
        pos.y = targetYPosition;
        transform.position = pos;
    }

    protected override void FixedUpdate()
    {
        _animator.SetBool("pacman", currentAttack == AttackType.Pacman);
        _animator.SetBool("scream", currentAttack == AttackType.Waves);
        if (currentAttack == AttackType.Pacman)
        {
            if (raw.ready)
            {
                raw.SingleAttack();
                raw.ResetReload();
            }
        }else if (currentAttack == AttackType.Waves)
        {
            waveSpawnReload -= Time.deltaTime;
            if (waveSpawnReload <= 0f)
            {
                SpawnWave();
                waveSpawnReload = timeBetweenWaveAttacks;
            }
        }
    }

    protected override void ChooseNewState()
    {
        rigidbody.velocity = Vector2.zero;
        if (currentAttack == AttackType.Jump)
        {
            if (jumpAttacksLeft-- <= 0)
            {
                falldamageDiler.SlashStop();
                SetState(startState);
                DestroyPortals();
                currentAttack = AttackType.Null;
            }
            else
            {
                JumpAttack();
            }
        }
        else if (currentAttack != AttackType.Null)
        {
            SetState(startState);
            currentAttack = AttackType.Null;

        }
        else
        {
            Vector2 dir = Player.Instance.transform.position - transform.position;
            ChooseAttack();
        }
    }


    #region Attacks
    private void ChooseAttack()
    {
        int attack = Random.Range(0, 4);
        if (attack == 0)
        {
            currentAttack = AttackType.Pacman;
            Pacman();
        }
        else if (attack == 1)
        {
            currentAttack = AttackType.Spheres;
            SpawnSpheres();
        }else if (attack == 2)
        {
            currentAttack = AttackType.Waves;
            WavesAttack();
        }
        else if (attack == 3)
        {
            currentAttack = AttackType.Jump;
            StartJumpAttack();
        }
}

    private void Pacman()
    {
        LookToPlayer();
        SetState(pacmanAttackState);
    }

    private void SpawnSpheres()
    {
        SetState(startState);
        foreach (var sphere in spheres)
        {
            sphere.SetActive(true);
        }
    }
    private void WavesAttack()
    {
        LookToPlayer();
        SetState(waveAttackState);
    }
    private void SpawnWave()
    {
        waveSpawner.Fire();
    }

    private void StartJumpAttack()
    {
        SpawnPortals();
        jumpAttacksLeft = jumpAttacks;
        SetState(flyState);
    }
    private void SpawnPortals()
    {
        for (int i = 0; i < portalsNum; i++)
        {
            SpawnPortal();
        }
    }

    private void DestroyPortals()
    {
        foreach (var portal in spawnedPortals)
        {
            Destroy(portal);
        }

        spawnedPortals = new List<GameObject>();
    }

    private void SpawnPortal()
    {
        float x = Player.Instance.transform.position.x;
        x += Random.Range(-portalsSpawnMaxDif, portalsSpawnMaxDif);
        Vector2 pos = new Vector2(x, battleZone.bounds.max.y);
        if (pos.x > battleZone.bounds.min.x && pos.x < battleZone.bounds.max.x)
        {
            GameObject portal = Instantiate(portalPrefab, pos, portalPrefab.transform.rotation);
            spawnedPortals.Add(portal);
        }
        else
        {
            SpawnPortal();
        }
        
    }

    private void JumpAttack()
    {
        falldamageDiler.SlashStop();
        falldamageDiler.SlashStart();
        int p = Random.Range(0, spawnedPortals.Count);
        Vector2 pos = spawnedPortals[p].transform.position;
        GameObject portal = spawnedPortals[p];
        spawnedPortals.RemoveAt(p);
        Destroy(portal);
        transform.position = pos;
        SetState(fallState);
    }
    #endregion

}
