using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Creature : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; private set; }
    public int health{ get; private set; }
    [field: SerializeField] public float timeToDie { get; private set; } // добавил для анимаций
    [SerializeField] private Vector2 impactForce;
    public bool isImpact { get; private set; } // Нужен, чтобы враг не бегал, когда он должен отлетать

    protected Rigidbody2D rigidbody;
    private List<Curse> _curses = new List<Curse>();

    virtual protected void Awake()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdateCurses", 0, 1);
    }
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

    /// <summary>
    /// Урон и хилл
    /// </summary>

    virtual public void GetDamage(int damage, Vector2 direction)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        StartCoroutine(GetImpact(direction));
        if (health <= 0) StartCoroutine(Die());
    }
    virtual public void GetDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
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

    /// <summary>
    /// Проклятия
    /// </summary>
    private void UpdateCurses()
    {
        List<Curse> toRemove = new List<Curse>();
        foreach (Curse curse in _curses)
        {
            if (curse.caster == null)
            {
                toRemove.Add(curse);
                continue;
            }
            curse.Activate();
        }

        foreach (var curse in toRemove)
        {
            _curses.Remove(curse);
        }
    }

    public void AttachCurse(Curse curse)
    {
        foreach (Curse cur in _curses)
        {
            if (cur.GetType() == curse.GetType() && cur.caster.gameObject == curse.caster.gameObject)
            {
                cur.stacks += curse.stacks;
                return;
            }
        }
        _curses.Add(curse);
    }

    public List<Curse> GetCurses()
    {
        return _curses;
    }

    public void ClearCursesFromCaster(CurseCaster caster)
    {
        List<Curse> toRemove = new List<Curse>();
        foreach (Curse curse in _curses)
        {
            if (curse.caster == caster)
            {
                toRemove.Add(curse);
            }
        }
        foreach (Curse curse in toRemove)
        {
            _curses.Remove(curse);
        }
    }

}

