public enum CurseType
{
    Fire, Water, Darkness,
}
public abstract class Curse
{
    public int stacks = 1;
    public CurseCaster caster;
    public abstract CurseType type { get; }
    public Creature owner { get; protected set; }

    public Curse(Creature target, CurseCaster curseCaster, int n)
    {
        caster = curseCaster;
        stacks = n;
        owner = target;
    }

    public virtual void Activate(){}
    public virtual void OnEnd(){}
}
