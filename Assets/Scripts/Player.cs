<<<<<<< Updated upstream
﻿using UnityEngine;
using UnityEngine.UI;
=======
﻿using System;
using System.Collections.Generic;
using Data.Util;
using UnityEngine;
>>>>>>> Stashed changes

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : Creature
{
    public static Player Instance;
    [SerializeField] private Sword weapon;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dashingSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] [Range(0, 1)] private float jumpInteruptionCoef = 0.2f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Image hpBar;

    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }

    private BoxCollider2D _collider;
    private Animator _animator;

    private const float GroundCheckDistance = 0.1f;

    private void Awake()
    {
        base.Awake();
        Instance = this;
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        HpBarUpdate();
    }
    private void FixedUpdate()
    {
        if (rigidbody.velocity.y <= 0)
        {
            isJumping = false;
            _animator.SetBool("jumping", isJumping);
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
        if (weapon.slashActive) return;
        
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
        rigidbody.velocity = new Vector2(vel, rigidbody.velocity.y);
    }
    

    public void Jump()
    {
        if (!isGrounded) return;
        isJumping = true;
        isGrounded = false;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        _animator.SetBool("jumping", isJumping);
    }

    public void StopJump()
    {
        if (isGrounded) return;
        if (rigidbody.velocity.y > 0)
        {
            isJumping = false;
            _animator.SetBool("jumping", isJumping);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y * jumpInteruptionCoef);
        }
    }

    public void StartBaseAttack()
    {
        _animator.SetTrigger("baseSwordAttack");
    }

    override public void GetDamage(int damage, Vector2 direction)
    {
        base.GetDamage(damage, direction);
        HpBarUpdate();
    }

    public void HpBarUpdate()
    {
        float amount = 1000 / maxHealth * health;
        hpBar.fillAmount = amount / 1000;
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
        
        // box должен быть чуть меньше чтобы избежать срабатываний при приблежении вплотную к стене
        Vector2 size = new Vector2(_collider.bounds.size.x - 0.001f, GroundCheckDistance);
        
        hit = Physics2D.BoxCast(positionToCheck, size, 0f, Vector2.down, GroundCheckDistance, groundLayerMask);
        if (hit) {
            isGrounded = true;
            isJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            // рисует box идентичный тому что используется для проверки isGrounded
            Vector2 size = new Vector2(_collider.bounds.size.x - 0.01f, GroundCheckDistance);
            Vector2 positionToCheck = _collider.bounds.center + _collider.bounds.extents.y * Vector3.down;

            if (!isGrounded) Gizmos.color = Color.red;
            else Gizmos.color = Color.green;

            Gizmos.DrawWireCube(positionToCheck, size);
        }
        catch
        {
            return;
        }
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
