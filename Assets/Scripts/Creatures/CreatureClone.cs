using UnityEngine;

// существо, имеющее общее здоровье с оригиналом (урон одному-урон другому)
public class CreatureClone : Creature
{
    [SerializeField] private Creature origin;
    public override void GetDamage(int damage)
    {
        origin.GetDamage(damage);
    }

    protected override void Awake()
    {
        base.Awake();
        maxHealth = origin.maxHealth;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        SetHealth(origin.health);
        if (origin == null)
        {
            Destroy(this.gameObject);
        }
    }
}
