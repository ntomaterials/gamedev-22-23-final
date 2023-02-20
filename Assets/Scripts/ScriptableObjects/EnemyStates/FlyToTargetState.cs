using System;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFlyToTargetState", menuName = "Custom/States/FlyToTargetState")]
public class FlyToTargetState : State
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Vector2 target;
    private float targetDistance = 0.2f;
    [SerializeField] private bool y;
    [SerializeField] private bool x;
    [SerializeField] private bool useLocalPosition = true;
    public override void Init(Enemy enemy)
    {
        base.Init(enemy);

        if (useLocalPosition)
        {
            target = target + (Vector2)owner.transform.parent.transform.position;
        }

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
        float xdist=0;
        float ydist=0;
        if (y) ydist = owner.transform.position.y - target.y;
        if (x) ydist = owner.transform.position.x - target.x;
        float dist = (float)Math.Sqrt(Math.Pow(ydist, 2) + Math.Pow(xdist, 2));
        if (dist <= targetDistance) isFinished = true;
        Vector2 move = dif.normalized * speed * Time.deltaTime;
        if (x)
        {
            owner.transform.position =  (Vector2)owner.transform.position + Vector2.right * move.x;
        }
        if (y)
        {
            owner.transform.position =  (Vector2)owner.transform.position + Vector2.up * move.y;
        }
    }
}
