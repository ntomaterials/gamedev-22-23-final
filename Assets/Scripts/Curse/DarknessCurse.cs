using UnityEngine;


public class DarknessCurse : Curse
{
    public DarknessCurse(Creature target, CurseCaster curseCaster, int n) : base(target, curseCaster, n){}
    public override CurseType type => CurseType.Darkness;

    private float totalIntens=0.2f;
    public override void Activate()
    {
        float change = 1f / (CursesManager.Instance.S) * 0.8f;
        totalIntens += change;
        CursesManager.Instance.vignette.intensity.value = totalIntens;
    }

    public override void OnEnd()
    {
        float newInten = CursesManager.Instance.vignette.intensity.value - totalIntens;
        CursesManager.Instance.vignette.intensity.value = newInten;
    }
}
