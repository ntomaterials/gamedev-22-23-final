using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : Creature
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private LayerMask groundLayerMask;
    
    public bool isGrounded { get; private set; }

    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private const float GroundCheckDistance = 0.01f;

    private void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// передвигает игрока по оси x
    /// </summary> 
    public void Run(float direction)
    {
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
        _animator.SetFloat("speed", Mathf.Abs(direction));
        _rigidbody.velocity = new Vector2(vel, _rigidbody.velocity.y);
    }
    

    public void Jump()
    {
        if (!isGrounded) return;
        isGrounded = false;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
        _animator.SetTrigger("jump");
    }

    public void Attack()
    {
        
    }
    
    private void OnCollisionStay2D(Collision2D collider)
    {
        CheckIfGrounded();
        _animator.SetBool("grounded", isGrounded);
    }
    private void OnCollisionExit2D(Collision2D collider)
    {
        isGrounded = false;
        _animator.SetBool("grounded", isGrounded);
    }
    private void CheckIfGrounded()
    {
        RaycastHit2D hit;
        Vector2 positionToCheck = _collider.bounds.center + _collider.bounds.extents.y * Vector3.down;
        Vector2 size = new Vector2(_collider.bounds.size.x - 0.1f, GroundCheckDistance);
        // box должен быть чуть меньше чтобы избежать срабатываний при приблежении вплотную к стене
        hit = Physics2D.BoxCast(positionToCheck, size, 0f, Vector2.down, GroundCheckDistance, groundLayerMask);
        if (hit) {
            isGrounded = true;
        }
    }

    private void OnDrawGizmos()
    {
        // рисует box идентичный тому что используется для проверки isGrounded
        Vector2 size = new Vector2(_collider.bounds.size.x - 0.01f, GroundCheckDistance);
        Vector2 positionToCheck = _collider.bounds.center + _collider.bounds.extents.y * Vector3.down;
        
        if (!isGrounded) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;

        Gizmos.DrawWireCube(positionToCheck, size);
    }
}
