using UnityEngine;

[CreateAssetMenu(fileName = "NewRunToMeetState", menuName = "Custom/States/RunToMeet")]
public class RunToMeetState : State
{
    [SerializeField] private bool changeSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float targetDistance = 0.1f;
    private int xDir = 1;

    public override void Run()
    {
        Wolf wolf=owner.GetComponent<Wolf>();
        if(wolf==null)
        {
            Debug.LogError("NoWolf "+owner.name);
            return;
        }
        try{
        Vector3 targetPos=wolf.target.transform.position;
        float dist = owner.transform.position.x - wolf.target.transform.position.x;
        if (targetPos.x - owner.transform.position.x > 0) xDir = 1;
        else xDir = -1;

        if(wolf.target==null)
        {
            //wolf.target=null;
            isFinished=true;  
        } 
        else if (Mathf.Abs(dist) <= targetDistance)
        {
            owner.RotateByX(xDir);
            isFinished = true;
            wolf.TakeMeet(); 
        }
        else
        {
            if (!owner.canMove) owner.Run(0f);
            else
            {
                if (owner.CheckEdge()) owner.Run(xDir, 0);
                else
                {
                    if (changeSpeed) owner.Run(xDir, speed);
                    else owner.Run(xDir);
                }
            }  
        }
        }
        catch
        {
            wolf.target=null;
            wolf.runToMeet=false;
            isFinished=true;
        }
    }
}