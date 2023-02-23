using UnityEngine;

public class Squirrel : Enemy
{
    [SerializeField] private WaitingState waitingState;
    [SerializeField] private Sword weapon;
    [SerializeField] private Vector2 jumpForce;

    [SerializeField] private float attackReload=2f;

    private float reloadTime=0f;
    private bool _attaking=false;
    


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

    protected void FixedUpdate()
    {
        if (_attaking && isGrounded)
        {
            SetState(waitingState);
            _attaking = false;
        }

        reloadTime -= Time.fixedDeltaTime;
    }

    private void Attack()
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
