using UnityEngine;

[CreateAssetMenu(fileName = "NewFlyToTargetState", menuName = "Custom/States/FlyToTargetState")]
public class FlyToTargetState : State
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Vector2 target;
    private float targetDistance = 0.2f;
    [SerializeField] private bool y;
    [SerializeField] private bool x;
    public override void Init(Enemy enemy)
    {
        owner = enemy;
        if (!x)
        {
            target = new Vector2(owner.transform.position.x, target.y);
        }
        if (!y)
        {
            target = new Vector2(target.x, owner.transform.position.y);
        }
    }
    

    public override void Run()
    {
        Vector2 dif = target - (Vector2)owner.transform.position;
        if (dif.magnitude <= targetDistance) isFinished = true;
        Vector2 move = dif.normalized * speed * Time.deltaTime;
        owner.transform.position = (Vector2)owner.transform.position + move;
    }
}
