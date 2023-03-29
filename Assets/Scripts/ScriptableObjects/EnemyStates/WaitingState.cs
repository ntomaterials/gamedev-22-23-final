using UnityEngine;

[CreateAssetMenu(fileName = "NewWaitingState", menuName = "Custom/States/WaitingState")]
public class WaitingState : State
{
    public float waitingTime = 3f;
    public bool infinity = false;
    public override void Init(Enemy enemy)
    {
        owner = enemy;
    }

    public override void Run()
    {
        if (!owner.isImpact && owner.isGrounded && owner.canMove)
        {
            owner.Run(0f);
        }
        
        if (infinity) return;
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0) isFinished = true;
    }
}
