using UnityEngine;

public class WaterCurse : Curse
{
    public WaterCurse(Creature target, CurseCaster curseCaster, int n) : base(target, curseCaster, n){}
    public override CurseType type => CurseType.Water;

    
    public override void Activate()
    {
        
    }
}
