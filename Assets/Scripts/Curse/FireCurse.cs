﻿public class FireCurse : Curse
{
    public FireCurse(Creature target, CurseCaster curseCaster, int n) : base(target, curseCaster, n){}
    public override CurseType type => CurseType.Fire;

    public override void Activate()
    {
        owner.GetDamage(stacks);
    }
}