using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolState", menuName = "Custom/States/PatrolState")]
public class PatrolState : State
{
    [SerializeField] private bool changeSpeed;

    [Tooltip("Set zero if you dont wont this to happend")] [SerializeField]
    private float stopWhenSeePlayerRadius = 0f;
    [SerializeField] private float speed;
    [SerializeField] private bool flipWhenNearToPlayer;
    
    protected int xDir = 1;
    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        if (owner.transform.rotation.eulerAngles.y == 180) xDir = -1;
    }

    public override void Run()
    {
        if (flipWhenNearToPlayer)
        {
            float dist = owner.transform.position.x - Player.Instance.transform.position.x;
            Vector3 playerPos = Player.Instance.transform.position;
            if (playerPos.x - owner.transform.position.x > 0) xDir = 1;
            else xDir = -1;
        }
        
        if (owner.CanSeePlayer(stopWhenSeePlayerRadius))
        {
            isFinished = true;
        }
        if (owner.MustTurn())
        {
            Turn();
        }

        if (owner.canMove)
        {
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

    protected virtual void Turn()
    {
        xDir *= -1;
        owner.Run(xDir, speed);
    }
}
