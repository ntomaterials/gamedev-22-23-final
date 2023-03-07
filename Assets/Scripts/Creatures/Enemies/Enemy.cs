using System;
using UnityEngine;
public class Enemy : Creature
{
    [SerializeField] private int dieXp;
    [field: SerializeField] public int touchDamage { get; private set; }
    [SerializeField] protected float knockbackPower = 1.5f;
    [SerializeField] protected State startState;
    protected State currentState;
    
    
    private float checkRadius=0.05f;
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        SetState(startState);
        player = FindObjectOfType<Player>();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (touchDamage == 0) return;
        if (GlobalConstants.PlayerLayer == collision.gameObject.layer)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Vector2 dir;

            if (player.transform.position.x >= transform.position.x) dir = new Vector2(knockbackPower, knockbackPower);
            else dir = new Vector2(-knockbackPower, knockbackPower * 0.2f);
            player.GetDamage(touchDamage, dir);
        }
    }

    protected void Update()
    {
        UpdateStates();
        if (!canMove)
        {
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
    }

    private void UpdateStates()
    {
        if (!currentState.isFinished) currentState.Run();
        else ChooseNewState();
    }

    /// <summary>
    /// Выбирает новое состояние согласно условиям, по умолчанию всегда выбирает startState
    /// </summary>
    protected virtual void ChooseNewState()
    {
        SetState(startState);
    }

    protected void SetState(State state)
    {
        currentState = Instantiate(state);
        currentState.Init(this);
    }
    # region Movement
    public void Flip()
    {
        transform.Rotate(Vector3.up, 180);
    }
    
    # endregion
    # region Check func

    public bool MustTurn()
    {
        return CheckWall() || (CheckEdge() && isGrounded);
    }

    public bool CanSeePlayer(float dist)
    {
        Vector2 dir = Player.Instance.transform.position - transform.position;
        if (dir.magnitude > dist) return false;
        if (Physics2D.Raycast(transform.position, dir, dir.magnitude, groundLayerMask)) return false;
        else return true;
        
    }
    public bool CanSeePlayer()
    {
        return CanSeePlayer(int.MaxValue);
    }

    public bool CheckWall()
    {
        Collider2D hit;
        
        Vector2 size = new Vector2(0.02f, collider.bounds.size.y* 0.8f * transform.localScale.x);
        Vector2 positionToCheck = collider.bounds.center + collider.bounds.extents.x*transform.localScale.x * transform.right + transform.right * size.x;

        hit = Physics2D.OverlapBox(positionToCheck, size, 0f,  groundLayerMask, -1f, 1f);
        if (hit != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        try
        {
            Vector2 size = new Vector2(0.01f, collider.bounds.size.y* 0.9f);
            Vector2 positionToCheck = collider.bounds.center +
                                      collider.bounds.extents.x * transform.localScale.x * transform.right +
                                      transform.right * size.x / 2;
            if (CheckWall())
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireCube(positionToCheck, size);
            
            Vector3 pos = transform.position + transform.right * collider.bounds.extents.x*(transform.localScale.x + 0.2f) +
                          Vector3.down * ( 0.1f + collider.bounds.extents.y - collider.offset.y);
            Collider2D[] cols = Physics2D.OverlapCircleAll(pos, checkRadius, groundLayerMask);
            if (cols.Length == 0)
            {
                Gizmos.color = Color.red;
            }else Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pos, checkRadius);
        }
        catch { return; }
    }

    public bool CheckEdge()
    {
        bool mustTurn=false;
        Vector3 pos = transform.position + transform.right * collider.bounds.extents.x*(transform.localScale.x + 0.2f) +
                      Vector3.down * ( 0.1f + collider.bounds.extents.y - collider.offset.y);
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, checkRadius, groundLayerMask);
        if (cols.Length == 0) mustTurn = true;
        return mustTurn;
    }
    # endregion
    
    public override void Die()
    {
        player.GetXp(dieXp);
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        collider.isTrigger = true;
        this.enabled = false;
        base.Die();
    }
    protected void LookToPlayer()
    {
        float xDir = Player.Instance.transform.position.x - transform.position.x;
        RotateByX(xDir);
    }
}
