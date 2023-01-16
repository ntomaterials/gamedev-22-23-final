public class BowTrap : Bow
{
    protected override void Update()
    {
        base.Update();
        if (reloadTime < 0f) Fire();
    }
}
