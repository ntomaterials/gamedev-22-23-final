using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnPatrol : Enemy
{
    [SerializeField] private float speed;
    private float factSpeed;
    [SerializeField] private Transform edgeChecker;
    private  const float checkRadius=0.15f;
    [SerializeField] private LayerMask groundLayerMask;
    public bool isGrounded { get; private set; }

    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private bool _isRight;
    private bool _isAlive;

    private const float GroundCheckDistance = 0.1f;

    private void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _isRight = true;
        _isAlive = true;

        factSpeed = speed;
    }
    private void FixedUpdate()
    {
       if(_isAlive) Run();
       if (isGrounded && MustTurn()) Flip();
    }
    private void Run()
    {
        if (isGrounded && !isImpact)
        {
            _rigidbody.velocity = new Vector2(factSpeed, _rigidbody.velocity.y);
        }
    }
    private void Flip()
    {
        _isRight = !_isRight;
        if(_isRight) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);
        factSpeed *= -1;
    }
    // IsGrounded меняется только при входе и выходе из коллайдера
    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        CheckIfGrounded();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        CheckIfGrounded();
    }
    private void CheckIfGrounded()
    {
        Vector2 positionToCheck = _collider.bounds.center + _collider.bounds.extents.y * Vector3.down;

        Collider2D hit = Physics2D.OverlapCircle(positionToCheck, GroundCheckDistance, groundLayerMask);
        if (hit) isGrounded = true;
        else isGrounded = false;
    }
    private bool MustTurn()
    {
        bool mustTurn=false;
        Collider2D[] cols = Physics2D.OverlapCircleAll(edgeChecker.position, checkRadius, groundLayerMask);
        if (cols.Length == 0) mustTurn = true;
        else
        {
            foreach (var col in cols)
            {
                    if ((col.bounds.center + col.bounds.extents.y * Vector3.up).y > edgeChecker.position.y) mustTurn = true;
                    else continue;
            }
        }
        return mustTurn;
    }
    // Вызывается триггером анимации. 
    //Можно вынести в общий класс существа
    public void OnDie()
    {
        _isAlive=false;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _collider.isTrigger = true;
    }
    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(edgeChecker.position, checkRadius);
        }
        catch
        {
            return;
        }
    }
}
