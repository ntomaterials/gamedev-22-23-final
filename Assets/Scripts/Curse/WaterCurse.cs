using System;
using UnityEngine;

public class WaterCurse : Curse
{
    public WaterCurse(Creature target, CurseCaster curseCaster, int n) : base(target, curseCaster, n){}
    public override CurseType type => CurseType.Water;
    private Rigidbody2D ownerRigidbody;
    private float force = 0.1f;
    private float totalSlow = 0f;
    
    public override void Activate()
    {
        float dicreaseSpeed = owner.speed * force;
        totalSlow += dicreaseSpeed;
        owner.speed = owner.speed -= dicreaseSpeed;
    }

    public override void OnEnd()
    {
        owner.speed += totalSlow;
    }
}
