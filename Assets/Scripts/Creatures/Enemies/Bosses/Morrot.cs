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
    
    [Space(5)]
    [Header("Balance")] 
    [SerializeField] private float canSeeDistance = 10f;
    [SerializeField] private float magicReload = 5f;
    [SerializeField] private float dashingTime = 3f;
    [SerializeField] private int spheres=4;
    [SerializeField] private Vector2 spheresHeightMaxMin;
    [SerializeField] private GameObject spherePrefab;


    private float _magicReloadTime = 3f;
    
    private enum AttackType {Null, Portals, Spheres}
    private AttackType currentAttack = AttackType.Null;
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
        int attack = Random.Range(1, 2);
        if (attack == 0)
        {
            currentAttack = AttackType.Portals;
            StartCoroutine(PortalsAttack());
        }else if (attack == 1)
        {
            currentAttack = AttackType.Spheres;
            StartCoroutine(SpheresAttack());
        }
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
