using System.Collections;
using System.Linq;
using UnityEngine;

public class Morrot : Enemy
{
    [Header("Init")]
    [SerializeField] private RunToPlayerState runToPlayerState;

    [SerializeField] private PacmanAttackState pacmanAttackState;
    [SerializeField] private WaitingState waitingState;
    [SerializeField] private Sword sword;

    [SerializeField] private GameObject[] portals;
    
    [Space(5)]
    [Header("Balance")] 
    [SerializeField] private float canSeeDistance = 10f;
    [SerializeField] private float magicReload = 5f;
    [SerializeField] private float dashingTime = 3f;


    private float _magicReloadTime = 3f;
    
    private enum AttackType {Null, Portals,}
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
        int attack = Random.Range(0, 1);
        if (attack == 0)
        {
            currentAttack = AttackType.Portals;
            StartCoroutine(PortalsAttack());
        }
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
