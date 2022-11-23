using UnityEngine;

public class Creature : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; }
    public int health{ get; private set; }

    virtual protected void Awake()
    {
        health = maxHealth;
    }

    virtual public void Die()
    {
        Destroy(this.gameObject);
    }

    virtual public void GetDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health <= 0) Die();
    }

    virtual public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

}

