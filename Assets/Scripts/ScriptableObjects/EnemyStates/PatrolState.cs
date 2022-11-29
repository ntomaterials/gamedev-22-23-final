using UnityEngine;

[CreateAssetMenu(fileName = "NewPatrolState", menuName = "Custom/States/PatrolState")]
public class PatrolState : State
{
    [SerializeField] private bool changeSpeed = true;
    [SerializeField] private float speed;
    public override void Run()
    {
        if (owner.MustTurn())
        {
            owner.Flip();
        }

        if (changeSpeed)
        {
            owner.Run(speed);
        }
        else
        {
            owner.Run();
        }
    }
}
