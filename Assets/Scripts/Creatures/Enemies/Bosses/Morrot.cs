using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Morrot : Enemy
{
    [Header("Init")]
    [SerializeField] [Tooltip("Attacks will spawn at this area")]  private Collider2D battleZone;
    [SerializeField] private RunToPlayerState runToPlayerState;

    [SerializeField] private PacmanAttackState pacmanAttackState;
    [SerializeField] private WaitingState waitingState;
    [SerializeField] private WaitingState infinityWaitingState;
    [SerializeField] private Sword sword;

    [SerializeField] private Transform teleportWhileSphereAttackPosition;
    [SerializeField] private GameObject[] portals;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private Transform laserAttackTeleportPoint;
    [SerializeField] private GameObject[] lasers;
    
    [Space(5)]
    [Header("Balance")] 
    [SerializeField] private float canSeeDistance = 10f;
    [SerializeField] private float magicReload = 5f;
    [SerializeField] private float dashingTime = 3f;
    [SerializeField] private int spheres=4;
    [SerializeField] private Vector2 spheresHeightMaxMin;
    [SerializeField] private int planetsCol = 8;
    [SerializeField] private float timeBetweenPlanetsSpawn = 0.2f;
    [SerializeField] private float planetSpawnHeightAmplintude = 2f;
    [SerializeField] private float laserAttackPreparationTime = 2f;
    [SerializeField] private float laserAttackDurationTime = 5f;


    private float _magicReloadTime = 3f;
    
    private enum AttackType {Null, Portals, Spheres, Asteroids, Lasers}
    private AttackType currentAttack = AttackType.Null;

    protected override void Awake()
    {
        base.Awake();
        SetLasersActive(false);
    }

    protected override void ChooseNewState()
    {
        Vector2 dir = Player.Instance.transform.position - transform.position;
        if (!CanSeePlayer(canSeeDistance))
        {
            SetState(startState);
        }
        else if (!sword.slashActive)
        {
            SetState(runToPlayerState);
        }
    }
    protected override void FixedUpdate()
    {
        _magicReloadTime -= Time.fixedDeltaTime;
        
        if (_magicReloadTime <= 0)
        {
            _magicReloadTime = magicReload;
            ChooseAttack();
        }
        
        if(sword.CheckTargets())
        {
            if (sword.ready && currentAttack == AttackType.Null)
            {
                animator.SetTrigger("meleeAttack");
                sword.ResetReload(); 
            }
        }
    }
    private void ChooseAttack()
    {
        int attack = Random.Range(0, 4);
        if (attack == 0)
        {
            currentAttack = AttackType.Portals;
            StartCoroutine(PortalsAttack());
        }else if (attack == 1)
        {
            currentAttack = AttackType.Spheres;
            StartCoroutine(SpheresAttack());
        }else if (attack == 2)
        {
            currentAttack = AttackType.Asteroids;
            StartCoroutine(AsteroidsAttack());
        }else if (attack == 3)
        {
            currentAttack = AttackType.Lasers;
            StartCoroutine(LasersAttack());
        }
    }

    private IEnumerator LasersAttack()
    {
        _magicReloadTime += laserAttackDurationTime + laserAttackPreparationTime;
        Player.Instance.transform.position = laserAttackTeleportPoint.position;
        yield return new WaitForSeconds(laserAttackPreparationTime);
        SetLasersActive(true);
        yield return new WaitForSeconds(laserAttackDurationTime);
        Player.Instance.transform.position = GetRandomObtainablePoint(0.5f, 0.5f);
        SetLasersActive(false);
        
        yield return new WaitForSeconds(0.5f); // мало ли впритык к боссу появится
        currentAttack = AttackType.Null;
    }
    private void SetLasersActive(bool active)
    {
        foreach (var laser in lasers)
        {
            laser.SetActive(active);
        }
    }

    private IEnumerator AsteroidsAttack()
    {
        _magicReloadTime += timeBetweenPlanetsSpawn * planetsCol;
        for (int i = 0; i < planetsCol; i++)
        {
            SpawnPlanet();
            yield return new WaitForSeconds(timeBetweenPlanetsSpawn);
        }

        currentAttack = AttackType.Null;
    }

    private void SpawnPlanet()
    {
        float x = Random.Range(battleZone.bounds.min.x,  battleZone.bounds.max.x);
        float y = Random.Range(battleZone.bounds.max.y - planetSpawnHeightAmplintude, battleZone.bounds.max.y);
        Vector2 startPos = new Vector2(x, y);
        Instantiate(planetPrefab, startPos, Quaternion.identity);
    }
    private IEnumerator SpheresAttack()
    {
        transform.position = teleportWhileSphereAttackPosition.position;
        SetState(infinityWaitingState);
        for (int i = 0; i < spheres; i++)
        {
            Vector2 pos = GetRandomObtainablePoint(spheresHeightMaxMin.x, spheresHeightMaxMin.y);
            GameObject sphere = Instantiate(spherePrefab, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(10f);
        currentAttack = AttackType.Null;
        transform.position = GetRandomObtainablePoint(0.5f, 1f);
        SetState(startState);
    }
    private IEnumerator PortalsAttack()
    {
        SetPortalsActive(true);
        foreach (int portalId in Enumerable.Range(0, portals.Length).OrderBy(x => Random.Range(0, 10)))
        {
            transform.position = portals[portalId].transform.position;
            LookToPlayer();
            
            animator.SetBool("dashing", true);
            SetState(pacmanAttackState);
            LookToPlayer();
            yield return new WaitForSeconds(dashingTime);
            sword.SlashStop();
            animator.SetBool("dashing", false);
            SetState(waitingState);
            yield return new WaitForSeconds(1f);
        }
        currentAttack = AttackType.Null;
        SetPortalsActive(false);
    }

    private void SetPortalsActive(bool active)
    {
        foreach (var portal in portals)
        {
            portal.SetActive(active);
        }
    }

    /// <summary>
    /// Генерирует случайную точку в боевой области, не выше чем maxHeight над землёй
    /// </summary>
    private Vector2 GetRandomObtainablePoint(float minHeight, float maxHeight)
    {
        float x = Random.Range(battleZone.bounds.min.x, battleZone.bounds.max.x);
        float y = Random.Range(battleZone.bounds.min.y, battleZone.bounds.max.y);
        Vector2 startPos = new Vector2(x, y);

        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, Mathf.Infinity, groundLayerMask);
        if (hit == null) throw new Exception("No ground");
        else
        {
            Vector2 pos = hit.point + Vector2.up * Random.Range(minHeight, maxHeight);
            return pos;
        }
    }
    
    #region Animation Triggers

    public void SlashStart()
    {
        sword.SlashStart();
    }
    public void SlashStop()
    {
        sword.SlashStop();
    }
    #endregion
}
