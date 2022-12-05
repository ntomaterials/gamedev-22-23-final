using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : Creature
{
    public static Player Instance;
    [SerializeField] private float weaponCooldown;///
    public Sword weapon;
    [SerializeField] private float dashingSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float immortalDuration = 1f;
    [Tooltip("Отсчёт идёт с момета конца кувырка")][SerializeField] private float rollReload=3f;
    [SerializeField] private float rollDuration = 0.5f; // надо подгонять под длительность анимации, ну или наоборот(
    [SerializeField] private float rollSpeed=3f;
    [SerializeField] [Range(0, 1)] private float jumpInteruptionCoef = 0.2f;
    [SerializeField] private Image hpBar;

    public bool isJumping { get; private set; }
    private bool isCooldown;//
    public bool blocking { get; private set; }

    private float _immortalTime = 0f;
    private int deafultLayer = GlobalConstants.PlayerLayer;
    private int immortalLayer = GlobalConstants.ImmortalLayer;
    private float _rollReloadTime;
    private float _blockReloadTime=0f;

    private void Awake()
    {
        base.Awake();
        Instance = this;
        HpBarUpdate();
    }
    private void FixedUpdate()
    {
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
        if (weapon.slashActive)
        {
            rigidbody.velocity = transform.right * dashingSpeed;
        }
    }
    # region Movement
    public override void Run(float direction)
    {
        // во время атаки нельзя менять направление движения
        if (weapon.slashActive) return;
        base.Run(direction);
    }


    protected override void CheckIfGrounded()
    {
        base.CheckIfGrounded();
        if (isGrounded) isJumping = false;
    }
    public void Jump()
    {
        if (!isGrounded || isImpact || blocking) return;
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
    //
    public void StartBaseAttack()
    {
        if (!isCooldown && !(_immortalTime>0))
        {
            animator.SetTrigger("baseSwordAttack");
            isCooldown = true;
            Invoke("OverCooldown", weaponCooldown);
        }
    }
    private void OverCooldown()
    {
        isCooldown = false;
    }
    //

    public void Roll()
    {
        if (_rollReloadTime > 0) return;;
        _rollReloadTime = rollReload + rollDuration;
        animator.SetTrigger("roll");
        StartCoroutine(GetImpact(new Vector2(rollSpeed * GetXDirection(), 0), rollDuration));
        BecomeImmortal(rollDuration);
    }
    public void Block()
    {
        if (!weapon.hasBlock) return;
        if (!(isImpact || weapon.slashActive) && _blockReloadTime <= 0)
        {
            _blockReloadTime = weapon.blockReload;
            animator.SetTrigger("block");
        }
    }
    # endregion
    override public void GetDamage(int damage, Vector2 direction)
    {
        if (_immortalTime > 0) return;
        if (blocking)
        {
            if (GetXDirection() * direction.x < 0) return; // если атака спереди
            else StopBlock();
        }
        BecomeImmortal();
        weapon.SlashStop();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
        weapon.SlashStart();
    }
    public void SlashStop()
    {
        weapon.SlashStop();
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
