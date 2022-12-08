using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Потом заменим событием смерти и вызовом окна смерти/возвращением к костру (Могу я этим заняться)
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : Creature
{
    public static Player Instance;

    public int playerXp { get; private set; }///
    //[SerializeField] private DeathMarker deathMarker;///
    //private SaveLoadManager saveLoadManager;//
    //private LevelsData levelsData;

    [SerializeField] private GameObject[] weapons;
    [SerializeField] private float dashingSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float immortalDuration = 1f;
    [Tooltip("Отсчёт идёт с момета конца кувырка")][SerializeField] private float rollReload=3f;
    [SerializeField] private float rollDuration = 0.5f; // надо подгонять под длительность анимации, ну или наоборот(
    [SerializeField] private float rollSpeed=3f;
    [SerializeField] [Range(0, 1)] private float jumpInteruptionCoef = 0.2f;
    [SerializeField] private Image hpBar;

    public bool isJumping { get; private set; }
    public bool blocking { get; private set; }

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
        Instance = this;
        HpBarUpdate();
        SetWeapon(weapons[0]);

        //saveLoadManager = FindObjectOfType<SaveLoadManager>();
        //levelsData = FindObjectOfType<LevelsData>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _rollReloadTime -= Time.fixedDeltaTime;
        _blockReloadTime -= Time.fixedDeltaTime;
        _immortalTime -= Time.fixedDeltaTime;
        
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
    }

    public void GetXp(int xp)
    {
        onXpChanged?.Invoke(xp);
        playerXp += xp;
    }
    public void ResetXp()
    {
        GetXp(-playerXp);
    }
    # region Movement
    public override void Run(float direction)
    {
        // во время атаки нельзя менять направление движения
        if (!canMove) return;
        base.Run(direction);
    }
    protected override void CheckIfGrounded()
    {
        base.CheckIfGrounded();
        if (isGrounded) isJumping = false;
    }
    public void Jump()
    {
        if (!isGrounded || isImpact || blocking || stunned) return;
        isJumping = true;
        isGrounded = false;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        animator.SetBool("jumping", isJumping);
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
    # endregion

    public void SetWeapon(GameObject newWeaponPrefab)
    {
        if (!canMove) return;
        if (currentWeapon != null) Destroy(currentWeapon.gameObject);
        GameObject newWeapon = Instantiate(newWeaponPrefab, transform);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        animatorL = currentWeapon.leftPlayerAnimation;
        animatorR = currentWeapon.rightPlayerAnimation;
    }

    public void SetWeapon(int id)
    {
        if (weapons.Length <= id) return;
        SetWeapon(weapons[id]);
    }

    # region Damage Health Die
    override public void GetDamage(int damage, Vector2 direction)
    {
        if (_immortalTime > 0) return;
        if (blocking)
        {
            if (GetXDirection() * direction.x < 0) return; // если атака спереди
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
    #endregion
}