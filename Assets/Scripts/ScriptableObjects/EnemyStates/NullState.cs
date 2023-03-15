using UnityEngine;

[CreateAssetMenu(fileName = "NewNullState", menuName = "Custom/States/NullState")]
public class NullState : State
{
    public override void Run()
    {
        owner.Run(0);
    }
}
