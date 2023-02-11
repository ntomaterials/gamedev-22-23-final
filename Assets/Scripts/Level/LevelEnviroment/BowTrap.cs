using UnityEngine;
[RequireComponent(typeof(Animator))]
public class BowTrap : Bow
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Update()
    {
        base.Update();
        if (reloadTime < 0f) Fire();
    }
    public override void Fire()
    {
        base.Fire();
        animator.SetTrigger("Shot");
    }
}
