using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sword : Weapon
{
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private Vector2 knockbackPower;

    private Collider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Наносит damage всем Creature в зоне _collider. Урон наносится не более 1 раза за 1 вызов функции
    /// </summary>
    private IEnumerator DamageWhileSlash()
    {
        List<Creature> hittedCreatures = new List<Creature>();
        while (slashActive)
        {
            List<Collider2D> hitColliders = new List<Collider2D>();
            Physics2D.OverlapCollider(_collider, contactFilter, hitColliders);

            foreach (var col in hitColliders)
            {
                Creature creature = col.GetComponent<Creature>();
                if (creature != null && !hittedCreatures.Contains(creature))
                {
                    hittedCreatures.Add(creature);
                    creature.GetDamage(1, new Vector2(transform.right.x, 0.5f) * knockbackPower); // направление отдачи - это направление меча
                }
            }
            yield return null;
        }
    }
    
    
    public override void SlashStart()
    {
        slashActive = true;
        reloadTime = reload;
        StartCoroutine(DamageWhileSlash());
    }
    public override void SlashStop()
    {
        slashActive = false;
    }
}
