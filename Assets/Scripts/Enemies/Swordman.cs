using UnityEngine;

public class Swordman : Enemy
{
    [SerializeField] private Collider2D attackZone;
    [SerializeField] private RunToPlayerState runToPlayerState;
    [SerializeField] private Sword weapon;
    [SerializeField] private LayerMask attackLayers;

    [SerializeField] private float reload=2f;

    private float reloadTime=0f;
    


    protected override void ChooseNewState()
    {
        Vector2 dir = Player.Instance.transform.position - transform.position;
        if (!CanSeePlayer())
        {
            SetState(startState);
        }
        else
        {
            SetState(runToPlayerState);
        }
    }

    protected void FixedUpdate()
    {
        reloadTime -= Time.fixedDeltaTime;
        if(Physics2D.OverlapCircle(attackZone.bounds.center, attackZone.bounds.extents.x, attackLayers) && reloadTime <= 0)
        {
            reloadTime = reload;
            animator.SetTrigger("attack");
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
