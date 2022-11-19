using UnityEngine;

public class Creature : MonoBehaviour
{
    public int health { get; private set; }

    protected virtual void Update()
    {
        if (health <= 0) Die();
    }

    virtual public void Die()
    {
        Destroy(this.gameObject);
    }
    

    virtual public void GetDamage(int damage)
    {
        health -= damage;
    }
}
