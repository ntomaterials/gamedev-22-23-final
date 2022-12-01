using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolState", menuName = "Custom/States/PatrolState")]
public class PatrolState : State
{
    [SerializeField] private bool changeSpeed;

    [Tooltip("Set zero if tou dont wont this to happend")] [SerializeField]
    private float stopWhenSeePlayerRadius = 0f;
    [SerializeField] private float speed;
    
    private int xDir = 1;
    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        if (owner.transform.rotation.eulerAngles.y == 180) xDir = -1;
    }

    public override void Run()
    {
        if (owner.CanSeePlayer(stopWhenSeePlayerRadius))
        {
            isFinished = true;
        }
        if (owner.MustTurn())
        {
            xDir *= -1;
        }

        if (changeSpeed)
        {
            owner.Run(xDir, speed);
        }
        else
        {
            owner.Run(xDir);
        }
    }
}
