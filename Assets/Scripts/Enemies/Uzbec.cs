using UnityEngine;

public class Uzbec : Enemy
{
    [Header("Init")]
    [SerializeField] private Collider2D attackZone;
    [SerializeField] private RunToPlayerState runToPlayerState;
    [SerializeField] private Sword sword;
    [SerializeField] private Bow bow;
    [SerializeField] private Bow magicCaster;
    [SerializeField] private LayerMask attackLayers;

    [Header("Balance")]
    [SerializeField] private float startDistanceAttackDistance = 1f;
    [SerializeField] private float startMagicAttackDistance = 3f;
    [SerializeField] private float startStompDistance = 6f;
    [SerializeField] private float stompReload = 5f; // stomp - топот
    [SerializeField] private int stompDamage = 1;

    private float _stompReloadTime = 0f;

    protected override void ChooseNewState()
    {
        Vector2 dir = Player.Instance.transform.position - transform.position;
        if (!CanSeePlayer())
        {
            SetState(startState);
        }
        else if (!sword.slashActive)
        {
            SetState(runToPlayerState);
        }
    }
    protected void FixedUpdate()
    {
        _stompReloadTime -= Time.fixedDeltaTime;
        float xDist = Player.Instance.transform.position.x - transform.position.x;
        if (!CanSeePlayer()) return;
        if(attackZone.bounds.Contains(Player.Instance.transform.position) && sword.ready)
        {
            animator.SetTrigger("baseSwordAttack");
        }else if (_stompReloadTime <= 0 && canMove && Mathf.Abs(xDist) >= startStompDistance)
        {
            _stompReloadTime = stompReload;
            animator.SetTrigger("stomp");
        }
        else if (magicCaster.ready && canMove && Mathf.Abs(xDist) >= startMagicAttackDistance)
        {
            animator.SetTrigger("magicAttack");
        }
        else if (bow.ready && canMove && Mathf.Abs(xDist) >= startDistanceAttackDistance)
        {
            animator.SetTrigger("shoot");
        }
    }

    #region Animation Triggers

    public void SlashStart()
    {
        sword.SlashStart();
    }
    public void SlashStop()
    {
        sword.SlashStop();
    }
    public void BowAttack()
    {
        bow.Fire();
    }
    public void MagicAttack()
    {
        magicCaster.Fire();
    }

    public void StompAttack()
    {
        float yDif = Player.Instance.transform.position.y - transform.position.y;
        if (Player.Instance.isGrounded && Mathf.Abs(yDif) <= 0.1f)
        {
            Player.Instance.GetDamage(stompDamage, Vector2.up * 5);
            Player.Instance.Stun(4f);
        }
    }
    #endregion
}