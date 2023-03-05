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
    [SerializeField] private Bow bow;
    [SerializeField] private Collider2D meleeAttackZone;
    [SerializeField] private WaitingState owlsAttackState;
    [SerializeField] private RunToPlayerState runToPlayerState;
    [SerializeField] private RunToPlayerState wolfState;
    [SerializeField]private List<ArcShooter> owls;
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private LayerMask attackLayers;
    [Header("Balance")] 
    [SerializeField] private float canSeeDistance = 10f;
    [SerializeField] private float magicReload = 5f;
    [SerializeField]float timeForOwlAttack = 1.5f;
    
    
    private float _magicReloadTime = 3f;
    private int _combo = 0;
    
    
    private enum AttackType{Null, Owls, Rocks, Wolf}

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
        
        if(Physics2D.OverlapCircle(meleeAttackZone.bounds.center, meleeAttackZone.bounds.extents.x, attackLayers))
        {
            if (sword.ready && currentAttack == AttackType.Null)
            {
               animator.SetTrigger("meleeAttack");
               sword.ResetReload(); 
            }
            }else if (bow.ready && currentAttack == AttackType.Null)
            {
                LookToPlayer();
                bow.ResetReload();
                animator.SetTrigger("rifleAttack");
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
        int attack = Random.Range(0, 3);
        if (attack == 0)
        {
            currentAttack = AttackType.Owls;
            StartCoroutine(OwlsAttack());
        }else if (attack == 1)
        {
            currentAttack = AttackType.Rocks;
            RocksAttack();
        }else if (attack == 2)
        {
            currentAttack = AttackType.Wolf;
            StartCoroutine(WolfAttack());
        }
    }

    private IEnumerator WolfAttack()
    {
        SetState(wolfState);
        currentAttack = AttackType.Wolf;
        animator.SetBool("transformation", true);
        _combo = 3;
        while (_combo > 0)
        {
            _magicReloadTime += Time.fixedDeltaTime;
            if (_combo == 2) sword.stunDuration = 1f;
            if (_combo != 2) sword.stunDuration = 0f;
            if (Physics2D.OverlapCircle(meleeAttackZone.bounds.center, meleeAttackZone.bounds.extents.x, attackLayers))
            {
                animator.SetTrigger("combo" + _combo.ToString());
                _combo -= 1;
                yield return new WaitForSeconds(0.2f);
            }
            
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        yield return new WaitForSeconds(1f);
        animator.SetBool("transformation", false);
        currentAttack = AttackType.Null;
        SetState(startState);
    }
    private void RocksAttack()
    {
        Vector2 pos;
        float x, y;
        y = battleZone.bounds.max.y;
        x = Random.Range(battleZone.bounds.min.x, battleZone.bounds.max.x);
        pos = new Vector2(x, y);
        GameObject rock = Instantiate(rockPrefab, pos, Quaternion.identity);
    }
    private IEnumerator OwlsAttack()
    {
        SetState(owlsAttackState);
        SetOwlsActive(true);
        
        animator.SetTrigger("whistle");
        
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

    public void Fire()
    {
        bow.Fire();
    }
    #endregion
}
