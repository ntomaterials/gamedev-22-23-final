using UnityEngine;

public class Enemy : Creature
{
    [field: SerializeField] public int touchDamage { get; private set; }
    [SerializeField] protected float knockbackPower = 1.5f;
    [SerializeField] protected State startState;
    protected State currentState;
    
    private float checkRadius=0.15f;
    
    protected override void Awake()
    {
        base.Awake();
        SetState(startState);
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
            else dir = new Vector2(-knockbackPower, knockbackPower * 0.5f);
            player.GetDamage(touchDamage, dir);
        }
    }

    protected void Update()
    {
        UpdateStates();
    }

    private void UpdateStates()
    {
        if (!currentState.isFinished) currentState.Run();
        else ChooseNewState();
    }

    /// <summary>
    /// Выбирает новое состояние согласно условиям, по умолчанию всегда выбирает staerState
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
        return (CheckWall() || CheckEdge()) && isGrounded;
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
        RaycastHit2D hit;
        Vector2 positionToCheck = collider.bounds.center + collider.bounds.extents.x * transform.right;
        
        Vector2 size = new Vector2(0.2f, collider.bounds.size.y - 0.01f);
        
        hit = Physics2D.BoxCast(positionToCheck, size, 0f, transform.right, 0.1f, groundLayerMask);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckEdge()
    {
        bool mustTurn=false;
        Vector3 pos = transform.position + transform.right * collider.bounds.extents.x + Vector3.down * collider.bounds.extents.y;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, checkRadius, groundLayerMask);
        if (cols.Length == 0) mustTurn = true;
        return mustTurn;
    }
    # endregion
    
    
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position + transform.right * collider.bounds.extents.x + Vector3.down * collider.bounds.extents.y;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, checkRadius, groundLayerMask);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(pos, checkRadius);
        
        
        Vector2 positionToCheck = collider.bounds.center + collider.bounds.extents.x * transform.right;
        Vector2 size = new Vector2(0.2f, collider.bounds.size.y - 0.01f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(positionToCheck, size);
    }
    public override void Die()
    {
        base.Die();
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        collider.isTrigger = true;
        this.enabled = false;
    }

}
