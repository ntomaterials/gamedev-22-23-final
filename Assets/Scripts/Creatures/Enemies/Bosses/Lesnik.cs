using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class Lesnik : Enemy
{
    [Header("Init")]
    [SerializeField] [Tooltip("Attacks will spawn at this area")]  private Collider2D battleZone;

    [SerializeField] private Sword sword;
    [SerializeField] private Collider2D meleeAttackZone;
    [SerializeField] private WaitingState owlsAttackState;
    [SerializeField] private RunToPlayerState runToPlayerState;
    [SerializeField]private List<ArcShooter> owls;
    [SerializeField] private LayerMask attackLayers;
    [Header("Balance")] 
    [SerializeField] private float canSeeDistance = 10f;
    [SerializeField] private float magicReload = 5f;
    [SerializeField]float timeForOwlAttack = 1.5f;
    
    
    private float _magicReloadTime = 3f;
    
    
    private enum AttackType{Null, Owls, }

    private AttackType currentAttack = AttackType.Null;
    
    private void Awake()
    {
        base.Awake();
        _magicReloadTime = magicReload / 2;
        SetOwlsActive(false);
    }
    

    protected override void FixedUpdate()
    {
        _magicReloadTime -= Time.fixedDeltaTime;
        
        if (_magicReloadTime <= 0)
        {
            _magicReloadTime = magicReload;
            ChooseAttack();
        }
        
        if(Physics2D.OverlapCircle(meleeAttackZone.bounds.center, meleeAttackZone.bounds.extents.x, attackLayers)
           && sword.ready && currentAttack == AttackType.Null)
        {
            animator.SetTrigger("attack");
            sword.ResetReload();
        }
    }

    protected override void ChooseNewState()
    {
        if (!CanSeePlayer(canSeeDistance))
        {
            SetState(startState);
        }
        else if (!sword.slashActive)
        {
            SetState(runToPlayerState);
        }
    }


    #region Attacks
    private void ChooseAttack()
    {
        int attack = Random.Range(0, 1);
        if (attack == 0)
        {
            currentAttack = AttackType.Owls;
            StartCoroutine(OwlsAttack());
        }
    }

    private IEnumerator OwlsAttack()
    {
        SetState(owlsAttackState);
        SetOwlsActive(true);
        
        int attacks = 3;
        int owlsPerAttack = 2;
        int i = 0;
        foreach (int owlId in Enumerable.Range(0, attacks * owlsPerAttack).OrderBy(x => Random.Range(0, 100)))
        {
            owls[owlId].Fire();
            i++;
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(timeForOwlAttack);
            }
           
        }

        SetOwlsActive(false);
        currentAttack = AttackType.Null;
    }
    #endregion

    private void SetOwlsActive(bool active)
    {
        foreach (var owl in owls)
        {
            owl.gameObject.SetActive(active);
        }
    }
    public override void Die()
    {
        SetOwlsActive(false);
        base.Die();
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
