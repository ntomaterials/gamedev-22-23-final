using UnityEngine;

[CreateAssetMenu(fileName = "NewWaitingState", menuName = "Custom/States/WaitingState")]
public class WaitingState : State
{
    public float waitingTime = 3f;
    public override void Init(Enemy enemy)
    {
        owner = enemy;
    }

    public override void Run()
    {
        waitingTime -= Time.deltaTime;
        if (waitingTime <= 0) isFinished = true;
    }
}
