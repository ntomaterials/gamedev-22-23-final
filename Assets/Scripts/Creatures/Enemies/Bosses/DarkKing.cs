using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class DarkKing : Enemy
{
    [Header("Init")]
    [SerializeField] [Tooltip("Attacks will spawn at this area")] 
    private Collider2D battleZone;
    [SerializeField] private Finish portal;

    [SerializeField] private GameObject head;
    [SerializeField] private Collider2D attackZone;
    [SerializeField] private RunToPlayerState runToPlayerState;
    [SerializeField] private State dancingState;
    [SerializeField] private Sword sword;
    [SerializeField] private GameObject magicCloudPrefab;
    [SerializeField] private Animator[] platformAnimators;
    [Space(5)]
    [SerializeField] private LayerMask attackLayers;
    

    [Header("Balance")]
    [SerializeField] private float canSeeDistance = 10f;

    [SerializeField] private float magicReload = 5f;
    [Header("magicCloudAttack")]
    [SerializeField] private float magicCloudAttackDuration = 10f;
    [SerializeField] private float magicCloudSpawnDelay = 2f;

    [Header("Lighting Attack")] [SerializeField]
    private Transform[] lightingAttackPositions;
    [SerializeField] private int numbersOfLightingAttack = 5;
    [SerializeField] private float timeBetweenLightingAttack = 1f;
    [SerializeField] private GameObject lightingWarningPrefab;
    [SerializeField] private GameObject floorLightingWarningPrefab;
    [SerializeField] private Transform floorLightingSpawnPosition;

    [Header ("Sounds")]
    [SerializeField] private AudioClip lightingSound;
    [SerializeField] private AudioClip SmehDedaSound;

    private float _magicReloadTime = 3f;
    private bool _dancing = false;
    private BossTrigger bossTrigger; // одноразовый
    private void Awake()
    {
        base.Awake();
        bossTrigger = FindObjectOfType<BossTrigger>();
        portal.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetActivePlatforms(false);
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
        base.FixedUpdate();
        _magicReloadTime -= Time.fixedDeltaTime;
        float xDist = Player.Instance.transform.position.x - transform.position.x;
        if (!CanSeePlayer(canSeeDistance)) return;
        if (_magicReloadTime <= 0)
        {
            ChooseMagicAttack();
        }
        else if (!_dancing){
            if (Physics2D.OverlapCircle(attackZone.bounds.center, attackZone.bounds.extents.x * 0.9f, attackLayers) && sword.ready)
            {
                sword.ResetReload();
                animator.SetTrigger("meleeAttack");
            }
        }
    }

    private void ChooseMagicAttack()
    {
        audioSource.PlayOneShot(SmehDedaSound);
        if (Random.Range(0, 2) == 0)
        {
            StartCoroutine(MagicClouds());
        }
        else
        {
            StartCoroutine(LightingAttack());
        }
        
    }

    #region Lights

    private IEnumerator LightingAttack()
    {
        audioSource.PlayOneShot(lightingSound);
        SetActivePlatforms(true);
        Instantiate(floorLightingWarningPrefab, floorLightingSpawnPosition.position,
            Quaternion.identity);
        
        _magicReloadTime = magicReload + numbersOfLightingAttack * timeBetweenLightingAttack;
        SetState(dancingState);
        animator.SetBool("laught", true);
        for (int i = 0; i < numbersOfLightingAttack; i++)
        {
            SpawnLighting();
            yield return new WaitForSeconds(timeBetweenLightingAttack);
        }
        SetState(runToPlayerState);
        SetActivePlatforms(false);
        
        animator.SetBool("laught", false);
        _dancing = false;
        audioSource.Stop();
    }

    private void SpawnLighting()
    {
        Vector2 pos = lightingAttackPositions[Random.Range(0, lightingAttackPositions.Length)].position;
        Instantiate(lightingWarningPrefab, pos, Quaternion.identity);
    }
    

    #endregion

    #region MagicClouds
    private IEnumerator MagicClouds()
    {
        _magicReloadTime = magicReload + magicCloudAttackDuration;
        SetState(dancingState);
        animator.SetBool("dance", true);
        _dancing = true;
        for (int i = 0; i < magicCloudAttackDuration / magicCloudSpawnDelay; i++)
        {
            SpawnMagicCloud();
            yield return new WaitForSeconds(magicCloudSpawnDelay);
        }
        SetState(runToPlayerState);
        animator.SetBool("dance", false);
        _dancing = false;
    }

    private void SpawnMagicCloud()
    {
        Vector2 pos = Tools.RandomPointInBounds(battleZone.bounds);
        GameObject magicCloud = Instantiate(magicCloudPrefab, pos, Quaternion.identity);
    }
    public override void Die()
    {
        head.transform.position = transform.position;
        head.SetActive(true);
        SetActivePlatforms(false);
        base.Die();
    }

    private void SetActivePlatforms(bool active)
    {
        foreach (var anim in platformAnimators)
        {
            anim.SetBool("active", active);
        }
    }
    #endregion

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
