using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : Creature
{
    public static Player Instance;
    [SerializeField] private Sword weapon;
    [SerializeField] private float weaponCooldown;///
    [SerializeField] private float dashingSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float immortalDuration = 1f;
    [SerializeField] [Range(0, 1)] private float jumpInteruptionCoef = 0.2f;
    [SerializeField] private Image hpBar;

    public bool isJumping { get; private set; }
    private bool isCooldown;//

    private Collider2D _collider;

    private float immortalTime = 0f;
    private int deafultLayer = GlobalConstants.PlayerLayer;
    private int immortalLayer = GlobalConstants.ImmortalLayer;

    private void Awake()
    {
        base.Awake();
        Instance = this;
        _collider = GetComponent<Collider2D>();
        HpBarUpdate();
    }
    private void FixedUpdate()
    {
        immortalTime -= Time.fixedDeltaTime;
        if (immortalTime <= 0)
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
        if (!isGrounded || isImpact) return;
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
        if (!isCooldown && !(immortalTime>0))
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
    override public void GetDamage(int damage, Vector2 direction)
    {
        if (immortalTime > 0) return;
        BecomeImmortal();
        animator.SetTrigger("damage");
        base.GetDamage(damage, direction);
        HpBarUpdate();
    }
    override public void GetDamage(int damage)
    {
        if (immortalTime > 0) return;
        BecomeImmortal();
        animator.SetTrigger("damage");
        base.GetDamage(damage);
        HpBarUpdate();
    }

    private void BecomeImmortal()
    {
        immortalTime = immortalDuration;
        gameObject.layer = immortalLayer;
    }

    public void HpBarUpdate()
    {
        float amount = 1000 / maxHealth * health;
        hpBar.fillAmount = amount / 1000;
    }
    public override void Die()
    {
        base.Die();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    #endregion
}
