using UnityEngine;

[CreateAssetMenu(fileName = "NewPacmanAttackState", menuName = "Custom/States/PacmanAttackState")]
public class PacmanAttackState: PatrolState
{
    [SerializeField] private int flips = 1;

    protected override void Turn()
    {
        flips -= 1;
        if (flips < 0)
        {
            isFinished = true;
        }
        base.Turn();
    }
}
