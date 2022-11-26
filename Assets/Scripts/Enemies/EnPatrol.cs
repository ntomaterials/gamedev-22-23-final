using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnPatrol : Enemy
{
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask groundLayerMask;
    public bool isGrounded { get; private set; }

    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private Vector2 _walkPoints; // Точки, между которыми перемещается х - левая, у - правая
    [SerializeField] private Vector2 _reservPoints;
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
    }
    private void FixedUpdate()
    {
       if(_isAlive) Run();
    }
    private void Run()
    {
        if (isGrounded && !isImpact)
        {
            float factSpeed = _speed;
            CheckFlip();
            if (_isRight) factSpeed = _speed;
            else factSpeed = -1 * _speed;

            _rigidbody.velocity = new Vector2(factSpeed, _rigidbody.velocity.y);
        }
    }
    private void CheckFlip()
    {
        if (transform.position.x >= _walkPoints.y)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _isRight = false;
        }
        if (transform.position.x <= _walkPoints.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _isRight = true;
        }
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

        Vector2 size = new Vector2(_collider.bounds.size.x - 0.001f, GroundCheckDistance);

        Collider2D [] hits = Physics2D.OverlapCircleAll(positionToCheck, GroundCheckDistance, groundLayerMask);
        if (hits.Length > 0)
        {
            isGrounded = true;
            //Получаем точки платформы, между которыми будем бегать
            try
            {
                Platform platform = hits[0].GetComponent<Platform>();
                _walkPoints = new Vector2(platform.Left.position.x, platform.Right.position.x);
            }
            //Такого, по-идее, вообще не должно быт
            catch
            {
                _walkPoints = _reservPoints;
            }
        }
        else isGrounded = false;
    }
    // Вызывается триггером анимации. 
    //Можно вынести в общий класс существа
    public void OnDie()
    {
        _isAlive=false;
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        //_rigidbody.bodyType=RigidbodyType2D.Static;
        _collider.isTrigger = true;
    }
}
