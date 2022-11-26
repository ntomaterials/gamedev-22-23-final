public class Curse
{
    public int stacks = 1;
    public CurseCaster caster;
    public Creature owner { get; private set; }

    public Curse(Creature target, CurseCaster curseCaster, int n)
    {
        caster = curseCaster;
        stacks = n;
        owner = target;
    }

    public virtual void Activate(){}
}
