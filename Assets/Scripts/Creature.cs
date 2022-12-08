using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Creature : MonoBehaviour
{
    [SerializeField ]protected AnimatorOverrideController animatorR;
    [SerializeField ]protected AnimatorOverrideController animatorL;
    [field: SerializeField] public int maxHealth { get; private set; }
    public float speed=3f;
    
    public LayerMask groundLayerMask;
    [field: SerializeField] public float timeToDie { get; private set; } // добавил для анимаций
    public bool effectedByKnockback = true;

    protected Animator animator;
    public int health{ get; private set; }
    public float deafultSpeed { get; private set; }
    public bool isGrounded { get; protected set; }
    private int _impactCol = 0;
    public bool isImpact // Нужен, чтобы враг не бегал, когда он должен отлетать
    {
        get { return _impactCol > 0; }
    }

    protected SpriteRenderer renderer;
    [HideInInspector]public bool canMove=true;

    protected Collider2D collider;
    protected Rigidbody2D rigidbody;

    private List<Curse> _curses = new List<Curse>();
    private float _stunTime = 0f;

    private const float GroundCheckDistance = 0.1f;

    public bool stunned
    {
        get
        {
            return _stunTime > 0;
        }
    }

    virtual protected void Awake()
    {
        deafultSpeed = speed;
        health = maxHealth;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        InvokeRepeating("UpdateCurses", 0, 1);
    }

    protected virtual void FixedUpdate()
    {
        if (xDirection == 1 && animatorR != null)
        {
            animator.runtimeAnimatorController = animatorR;
            renderer.flipX = false; // флип нужен тк без этого будет "двойной поворот" спрайта и transform
        }
        else if (animatorL != null) {
            animator.runtimeAnimatorController = animatorL;
            renderer.flipX = true;
        }
        _stunTime -= Time.fixedDeltaTime;
        if (!stunned) animator.SetBool("stun", false);
        if (stunned)
        {
            animator.SetFloat("speed", 0f);
        }

        if (stunned && !isImpact)
        {
            rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
        }
    }
    
    virtual public void Die() // Добавил время для проигрыша анимации
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        Destroy(this.gameObject, timeToDie);
    }
    /// <summary>
    /// Проверяет нахождения обьекта на земле
    /// </summary>
    protected virtual void CheckIfGrounded()
    {
        RaycastHit2D hit;
        Vector2 positionToCheck = collider.bounds.center + collider.bounds.extents.y * Vector3.down;
        
        // box должен быть чуть меньше чтобы избежать срабатываний при приблежении вплотную к стене
        Vector2 size = new Vector2(collider.bounds.size.x - 0.01f, GroundCheckDistance);
        
        hit = Physics2D.BoxCast(positionToCheck, size, 0f, Vector2.down, GroundCheckDistance, groundLayerMask);
        if (hit) {
            isGrounded = true;
        }
    }
    protected virtual void OnCollisionStay2D(Collision2D collider)
    {
        CheckIfGrounded();
    }

    protected virtual void OnCollisionExit2D(Collision2D collider)
    {
        isGrounded = false;
    }

    # region  Movement
    
    /// <summary>
    /// Вращает обьект по y в зависисмости от заданного направления по Х
    /// </summary>
    public virtual void RotateByX(float direction)
    {
        if (direction == 0) return;
        else if (direction > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }else if (direction < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    /// <summary>
    /// устанавливает скорость передвижения по оси x
    /// </summary> 
    public virtual void Run(float direction, float newSpeed)
    {
        if ( isImpact || !canMove || stunned) return;
        speed = newSpeed;
        
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

        if (direction != 0) animator.SetFloat("speed", Mathf.Abs(speed));
        else animator.SetFloat("speed", 0);
        
        rigidbody.velocity = new Vector2(vel, rigidbody.velocity.y);
    }
    
    public virtual void Run(float direction)
    {
        Run(direction, speed);
    }

    public int xDirection
    {
        get
        {
            if (transform.rotation.eulerAngles.y == 180)
            {
                return -1;
            }
            return 1;
        }
    }
    #endregion

    # region Damage and heal

    virtual public void GetDamage(int damage, Vector2 direction)
    {
        RotateByX(-direction.x);
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        _stunTime = 0f;
        StartCoroutine(GetImpact(direction));
        canMove = true;
        if (health <= 0) Die();
    }
    virtual public void GetDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        _stunTime = 0f;
        canMove = true;
        if (health <= 0) Die();
    }
    public IEnumerator GetImpact(Vector2 impact)
    {
        yield return GetImpact(impact, 0.1f);
    }
    public IEnumerator GetImpact(Vector2 impact, float duration)
    {
        if (effectedByKnockback)
        {
            _impactCol++;
            rigidbody.AddForce(Vector2.up*impact.y, ForceMode2D.Impulse);
            rigidbody.velocity = new Vector2(impact.x, rigidbody.velocity.y);
            yield return new WaitForSeconds(duration); // Долго махался с физицой, единственный рабочий вариант, который нашел. 
            _impactCol--;
        }
        while (rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = new Vector2(impact.x, rigidbody.velocity.y);
            yield return new WaitForSeconds(0.1f);
        }
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }

    public void Stun(float duration)
    {
        _stunTime = Mathf.Max(_stunTime, duration);
        animator.SetBool("stun", true);
        Run(0);
    }

    virtual public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
    virtual public void FullHeal()
    {
        health = maxHealth;
    }
    # endregion

    # region Curses
    private void UpdateCurses()
    {
        List<Curse> toRemove = new List<Curse>();
        foreach (Curse curse in _curses)
        {
            if (curse.caster == null)
            {
                toRemove.Add(curse);
                continue;
            }
            curse.Activate();
        }

        foreach (var curse in toRemove)
        {
            _curses.Remove(curse);
            curse.OnEnd();
        }
    }

    public void AttachCurse(Curse curse)
    {
        foreach (Curse cur in _curses)
        {
            if (cur.GetType() == curse.GetType() && cur.caster.gameObject == curse.caster.gameObject)
            {
                cur.stacks += curse.stacks;
                return;
            }
        }
        _curses.Add(curse);
    }

    public List<Curse> GetCurses()
    {
        return _curses;
    }

    public void ClearCursesFromCaster(CurseCaster caster)
    {
        List<Curse> toRemove = new List<Curse>();
        foreach (Curse curse in _curses)
        {
            if (curse.caster == caster)
            {
                toRemove.Add(curse);
            }
        }
        foreach (Curse curse in toRemove)
        {
            _curses.Remove(curse);
            curse.OnEnd();
        }
    }

    public int GetCursesStucksByType(CurseType type)
    {
        int n = 0;
        foreach (var cur in _curses)
        {
            if (cur.type == type) n += cur.stacks;
        }

        return n;
    }
    # endregion
    
    private void OnDrawGizmos()
    {
        try
        {
            // рисует box идентичный тому что используется для проверки isGrounded
            Vector2 size = new Vector2(collider.bounds.size.x - 0.01f, GroundCheckDistance);
            Vector2 positionToCheck = collider.bounds.center + collider.bounds.extents.y * Vector3.down;

            if (!isGrounded) Gizmos.color = Color.red;
            else Gizmos.color = Color.green;

            Gizmos.DrawWireCube(positionToCheck, size);
        }
        catch
        {
            return;
        }
    }
    # region Animation Events
    public void StopMove()
    {
        canMove = false;
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }
    public void ContinueMove()
    {
        canMove = true;
    }
    # endregion

}

