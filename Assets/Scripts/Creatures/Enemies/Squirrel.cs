using UnityEngine;

public class Squirrel : Enemy
{
    [SerializeField] protected WaitingState waitingState;
    [SerializeField] protected Sword weapon;
    [SerializeField] protected Vector2 jumpForce;

    [SerializeField] protected float attackReload=2f;

    protected float reloadTime=0f;
    protected bool _attaking=false;
    


    protected override void ChooseNewState()
    {
        Vector2 dir = Player.Instance.transform.position - transform.position;
        if (!CanSeePlayer())
        {
            SetState(startState);
            _attaking = false;
        }
        else if (isGrounded && reloadTime <= 0f)
        {
            Attack();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_attaking && isGrounded)
        {
            SetState(waitingState);
            _attaking = false;
        }

        reloadTime -= Time.fixedDeltaTime;
    }

    protected virtual void Attack()
    {
        animator.SetFloat("speed", 0f);
        animator.SetTrigger("attack");
        _attaking = true;
        SetState(waitingState);
        LookToPlayer();
        rigidbody.AddForce(transform.right * jumpForce.x + Vector3.up * jumpForce.y, ForceMode2D.Impulse);
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
