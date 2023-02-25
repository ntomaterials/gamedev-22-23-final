using UnityEngine;

public class Owl : Enemy
{
    [Space(5)] [Header("Owl")][SerializeField] private ArcShooter shooter;
    [SerializeField] private FlyOverGroundState flyState;
    [SerializeField] private WaitingState waitingState;
    private enum Mood{Waiting, Flying, Sleeping}

    private Mood currentMood = Mood.Sleeping;
    protected override void ChooseNewState()
    {
        if (currentMood == Mood.Sleeping)
        {
            SetState(flyState);
            currentMood = Mood.Flying;
        }else if (currentMood == Mood.Flying)
        {
            SetState(waitingState);
            shooter.Fire();
            currentMood = Mood.Waiting;
        }else if (currentMood == Mood.Waiting)
        {
            SetState(startState);
            currentMood = Mood.Sleeping;
        }
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetFloat("speed", rigidbody.velocity.magnitude);
    }
}
