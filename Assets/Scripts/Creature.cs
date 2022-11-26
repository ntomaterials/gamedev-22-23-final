using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///
[RequireComponent(typeof(Rigidbody2D))]
///
public class Creature : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; }
    public int health{ get; private set; }
    [field: SerializeField] public float timeToDie { get; private set; } // добавил для анимаций

    public Rigidbody2D rigidbody { get; private set; } // В теории, можно Rigidbody на всех существ вешать и получать его поле из этого класса
    [field: SerializeField] public Vector2 impactForce;
    public bool isImpact { get; private set; } // Нужен, чтобы враг не бегал, когда он должен отлетать

    virtual protected void Awake()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    /*virtual public void Die()
    {
        Destroy(this.gameObject);
    }*/
    virtual public IEnumerator Die() // Добавил время для проигрыша анимации
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Die");
            yield return new WaitForSeconds(timeToDie);
        }
        Destroy(this.gameObject);
    }


    virtual public void GetDamage(int damage, Vector2 direction)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        //
        StartCoroutine(GetImpact(direction));
        //
        if (health <= 0) StartCoroutine(Die());
    }
    private IEnumerator GetImpact(Vector2 direction)
    {
        isImpact = true;
        rigidbody.AddForce(Vector2.up*impactForce.y, ForceMode2D.Impulse);
        rigidbody.velocity = new Vector2(impactForce.x, rigidbody.velocity.y)*direction;
        yield return new WaitForSeconds(0.1f); // Долго махался с физицой, единственный рабочий вариант, который нашел. 
        isImpact = false;
    }

    virtual public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

}

