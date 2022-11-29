using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : Creature
{
    public static Player Instance;
    [SerializeField] private Sword weapon;
    [SerializeField] private float dashingSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] [Range(0, 1)] private float jumpInteruptionCoef = 0.2f;
    [SerializeField] private Image hpBar;

    public bool isJumping { get; private set; }

    private BoxCollider2D _collider;

    private void Awake()
    {
        base.Awake();
        Instance = this;
        _collider = GetComponent<BoxCollider2D>();
        HpBarUpdate();
    }
    private void FixedUpdate()
    {
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

    /// <summary>
    /// устанавливает скорость передвижения игрока по оси x
    /// </summary> 
    public void Run(float direction)
    {
        // во время атаки нельзя менять направление движения
        if (weapon.slashActive || isImpact) return;
        
        float vel = 0f;
        if (direction == 0) vel = 0f;
        else if (direction > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            vel = speed;
        }else if (direction < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            vel = -speed;
        }
        animator.SetFloat("speed", Mathf.Abs(direction));
        rigidbody.velocity = new Vector2(vel, rigidbody.velocity.y);
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

    public void StartBaseAttack()
    {
        animator.SetTrigger("baseSwordAttack");
    }

    override public void GetDamage(int damage, Vector2 direction)
    {
        animator.SetTrigger("damage");
        base.GetDamage(damage, direction);
        HpBarUpdate();
    }
    override public void GetDamage(int damage)
    {
        animator.SetTrigger("damage");
        base.GetDamage(damage);
        HpBarUpdate();
    }

    public void HpBarUpdate()
    {
        float amount = 1000 / maxHealth * health;
        hpBar.fillAmount = amount / 1000;
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
