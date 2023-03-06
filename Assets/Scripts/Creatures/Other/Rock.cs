using UnityEngine;

public class Rock : Creature
{
    protected virtual void FixedUpdate()
    {
        animator.SetInteger("damage", maxHealth - health);
    }
}
