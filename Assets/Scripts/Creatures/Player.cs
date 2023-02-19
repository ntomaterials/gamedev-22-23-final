using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Потом заменим событием смерти и вызовом окна смерти/возвращением к костру (Могу я этим заняться)
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerInventory))]
public class Player : Creature
{
    public static Player Instance;
    public static PlayerInventory Inventory;

    public int playerXp { get; private set; }///
    //[SerializeField] private DeathMarker deathMarker;///
    //private SaveLoadManager saveLoadManager;//
    //private LevelsData levelsData;
    [Header("Player")]
    [SerializeField] private PlayerWeaponInfo[] weaponsInfo;
    [field: SerializeField] public bool[] IsWeaponsActive { get; private set; }
    
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float immortalDuration = 1f;
    [Tooltip("Отсчёт идёт с момета конца кувырка")][SerializeField] private float rollReload=3f;
    [SerializeField] private float rollDuration = 0.5f; // надо подгонять под длительность анимации(
    [SerializeField] private float rollSpeed=3f;
    [SerializeField] [Range(0, 1)] private float jumpInteruptionCoef = 0.2f;
    [SerializeField] private float climbingSpeed = 1.5f;

    [Header("Init")]
    public Transform dropPoint;
    [SerializeField] private Image hpBar;
    
    private CapsuleCollider2D _capsuleCollider;
    [SerializeField] private PlayerColliderInfo deafultColliderInfo;
    [SerializeField] private PlayerColliderInfo crouchColliderInfo;
    [SerializeField] private PlayerColliderInfo rollColliderInfo;


    [SerializeField]
    private SpriteRenderer stunIndicator;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip rollSound;

    [SerializeField] private LayerMask climbingZoneLayerMask;

    public bool isJumping { get; private set; }
    public bool blocking { get; private set; }
    public bool crouching { get; private set; }
    public bool climbing { get; private set; }
    public bool canClimb { get; private set; }

    public event XpChanged onXpChanged;
    public delegate void XpChanged(int value);

    private float _immortalTime = 0f;
    private int deafultLayer = GlobalConstants.PlayerLayer;
    private int immortalLayer = GlobalConstants.ImmortalLayer;
    private float _rollReloadTime;
    private float _blockReloadTime=0f;
    private Weapon currentWeapon;
    

    private void Awake()
    {
        base.Awake();
        _capsuleCollider = (CapsuleCollider2D)collider;
        Instance = this;
        Inventory = GetComponent<PlayerInventory>();
        HpBarUpdate();
        SetWeapon(weaponsInfo[0]);

        //saveLoadManager = FindObjectOfType<SaveLoadManager>();
        //levelsData = FindObjectOfType<LevelsData>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckCanClimbing();
        _rollReloadTime -= Time.fixedDeltaTime;
        _blockReloadTime -= Time.fixedDeltaTime;
        _immortalTime -= Time.fixedDeltaTime;

        stunIndicator.enabled = stunned;
        
        if (_immortalTime <= 0)
        {
            gameObject.layer = deafultLayer;
        }
        if (rigidbody.velocity.y <= 0)
        {
            isJumping = false;
            animator.SetBool("jumping", isJumping);
        }
        if (currentWeapon.slashActive)
        {
            rigidbody.velocity = transform.right * currentWeapon.dashSpeed;
        }
        
        if (!canClimb && climbing) StopClimbing();
    }

    public void GetXp(int xp)
    {
        onXpChanged?.Invoke(xp);
        playerXp += xp;
    }
    public void SpendXp(int xp)
    {
        onXpChanged?.Invoke(xp);
        playerXp -= xp;
    }
    public void ResetXp()
    {
        GetXp(-playerXp+50);
    }
    # region Movement
    public override void Run(float direction)
    {
        // во время атаки нельзя менять направление движения
        if (!canMove || crouching) return;
        base.Run(direction);
    }
    protected override void CheckIfGrounded()
    {
        base.CheckIfGrounded();
        if (isGrounded) isJumping = false;
    }
    public void Jump(bool checkGrounded)
    {
        if (checkGrounded &&(!isGrounded || isImpact || blocking || stunned || crouching)) return;
        isJumping = true;
        isGrounded = false;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        animator.SetBool("jumping", isJumping);
        if(jumpSound!=null) audioSource.PlayOneShot(jumpSound);
    }

    public void Jump()
    {
        Jump(true);
    }

    public void StopJump()
    {
        if (isGrounded || isImpact) return;
        if (rigidbody.velocity.y > 0)
        {
            isJumping = false;
            animator.SetBool("jumping", isJumping);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * jumpInteruptionCoef);
        }
    }

    public void StartBaseAttack()
    {
        if (currentWeapon.ready && !stunned)
        {
            animator.SetTrigger("attack");
        }
    }

    public void Roll()
    {
        if (_rollReloadTime > 0 || stunned || isImpact || !canMove) return;;
        BecomeImmortal(rollDuration);
        
        animator.SetTrigger("roll");
        _rollReloadTime = rollReload + rollDuration;
        StartCoroutine(GetImpact(new Vector2(rollSpeed * GetXDirection(), 0), rollDuration));
        if(rollSound!=null) audioSource.PlayOneShot(rollSound);
        
        SetPlayerCollider(rollColliderInfo);
        StartCoroutine(SetPlayerCollider(deafultColliderInfo, rollDuration));
    }
    public void Block()
    {
        if (!currentWeapon.hasBlock) return;
        if (!(isImpact || currentWeapon.slashActive) && _blockReloadTime <= 0 && !stunned)
        {
            Run(0);
            _blockReloadTime = currentWeapon.blockReload;
            animator.SetTrigger("block");
        }
    }

    public void StartCrouch()
    {
        moveDirection = 0;
        crouching = true;
        SetPlayerCollider(crouchColliderInfo);
        animator.SetBool("crouch", true);
    }

    public void StopCrouch()
    {
        if (!crouching) return;
        crouching = false;
        SetPlayerCollider(deafultColliderInfo);
        animator.SetBool("crouch", false);
    }

    public void StartClimbing()
    {
        if (climbing) return;
        if (!canMove || crouching) return;
        
        RotateByX(1);
        rigidbody.velocity = Vector2.zero;
        climbing = true;
        rigidbody.isKinematic = true;
        animator.SetBool("climbing", true);
    }
    public void StopClimbing()
    {
        climbing = false;
        rigidbody.isKinematic = false;
        animator.SetBool("climbing", false);
    }

    private void CheckCanClimbing()
    {
        canClimb = Physics2D.OverlapCircle(transform.position, 0.1f, climbingZoneLayerMask);
    }

    public void Climb(Vector2 direction)
    {
        transform.Translate(direction.normalized * climbingSpeed * Time.deltaTime);
    }

    # endregion
    private void SetPlayerCollider(PlayerColliderInfo info)
    {
        _capsuleCollider.size = info.size;
        _capsuleCollider.offset = info.offset;
    }

    private IEnumerator SetPlayerCollider(PlayerColliderInfo info, float time)
    {
        yield return new WaitForSeconds(time);
        
        SetPlayerCollider(info);
    }

    public void SetWeapon(PlayerWeaponInfo weaponInfo)
    {
        if (!canMove) return;
        if (currentWeapon != null) Destroy(currentWeapon.gameObject);
        GameObject newWeapon = Instantiate(weaponInfo.weaponPrefab, transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        animatorL = weaponInfo.leftPlayerAnimation;
        animatorR = weaponInfo.rightPlayerAnimation;
    }

    public void SetWeapon(int id)
    {
        if (weaponsInfo.Length <= id) return;
        SetWeapon(weaponsInfo[id]);
    }
    public void ActiveNewWeapon(int id)
    {
        IsWeaponsActive[id] = true;
    }

    # region Damage Health Die
    override public void GetDamage(int damage, Vector2 direction)
    {
        if (_immortalTime > 0) return;
        if (blocking)
        {
            if (GetXDirection() * direction.x < 0)
            {
                return;
            } // если атака спереди
            else
            {
                StopBlock();
                canMove = true;
            }
        }
        BecomeImmortal();
        currentWeapon.SlashStop();
        blocking = false;
        animator.SetTrigger("damage");
        
        StopClimbing();
        base.GetDamage(damage, direction);
        HpBarUpdate();
    }
    override public void GetDamage(int damage) // используется для получения урона не от обычных атак (эффекты например)
    {
        if (_immortalTime > 0) return;
        base.GetDamage(damage);
        HpBarUpdate();
    }

    private void BecomeImmortal()
    {
        _immortalTime = immortalDuration;
        gameObject.layer = immortalLayer;
    }
    private void BecomeImmortal(float time)
    {
        _immortalTime = time;
        gameObject.layer = immortalLayer;
    }

    public void HpBarUpdate()
    {
        float amount = 1000 / maxHealth * health;
        hpBar.fillAmount = amount / 1000;
    }
    public void FullHeal()
    {
        Heal(maxHealth);
        HpBarUpdate();
    }
    public override void Die()
    {
        base.Die();
        /*DeathMarker marker = Instantiate(deathMarker.gameObject, transform.position, Quaternion.identity).GetComponent<DeathMarker>();
        marker.Score = playerXp;
        saveLoadManager.deathMarker = marker;
        saveLoadManager.deathMarkerPos = marker.transform;
        ResetXp();
        levelsData.LoadLevel(levelsData.lastSavedLevelID);*/
        ResetXp();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    # endregion

    public int GetXDirection()
    {
        if (transform.rotation.eulerAngles.y == 0) return 1;
        else return -1;
    }

    protected override void OnCollisionStay2D(Collision2D collider)
    {
        base.OnCollisionStay2D(collider);
        animator.SetBool("grounded", isGrounded);
    }

    protected override void OnCollisionExit2D(Collision2D collider)
    {
        base.OnCollisionExit2D(collider);
        animator.SetBool("grounded", isGrounded);
    }

    #region Animation Triggers

    public void SlashStart()
    {
        currentWeapon.SlashStart();
    }
    public void SlashStop()
    {
        currentWeapon.SlashStop();
    }

    public void Fire()
    {
        currentWeapon.Fire();
        animator.ResetTrigger("attack");
    }
    public void StartBlock()
    {
        blocking = true;
    }

    public void StopBlock()
    {
        blocking = false;
    }
    public void StartRoll()
    {
        blocking = true;
    }

    public void StopRoll()
    {
        blocking = false;
    }
    #endregion
}

[System.Serializable]
public class PlayerWeaponInfo
{
    public GameObject weaponPrefab;
    public AnimatorOverrideController leftPlayerAnimation;
    public AnimatorOverrideController rightPlayerAnimation;
}

[System.Serializable]
public class PlayerColliderInfo
{
    public Vector2 size=new Vector2(0.28f, 0.8f);
    public Vector2 offset=new Vector2(0, -0.1f);
}