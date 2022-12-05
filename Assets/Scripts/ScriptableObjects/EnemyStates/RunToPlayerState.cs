using UnityEngine;

[CreateAssetMenu(fileName = "NewRunToPlayerState", menuName = "Custom/States/RunToPlayer")]
public class RunToPlayerState : State
{
    [SerializeField] private bool changeSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float targetDistance = 0.1f;

    private int xDir = 1;

    public override void Run()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        float dist = owner.transform.position.x - Player.Instance.transform.position.x;
        if (playerPos.x - owner.transform.position.x > 0) xDir = 1;
        else xDir = -1;
        
        if (!owner.CanSeePlayer() || Mathf.Abs(dist) <= targetDistance)
        {
            owner.RotateByX(xDir);
            isFinished = true;
            owner.Run(0); // включение анимации покоя
        }
        else
        {
            if (!owner.canMove)
            {
                owner.Run(0f);
            }
            else
            {
                if (owner.CheckEdge())
                {
                    owner.Run(xDir, 0);
                }
                else
                {
                    if (changeSpeed){owner.Run(xDir, speed);}
                    else owner.Run(xDir);
                }
        }
            
            
        }
        
    }
}
