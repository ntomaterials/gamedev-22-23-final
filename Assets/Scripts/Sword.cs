using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Sword : MonoBehaviour
{
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private float damage;
    
    private Collider2D _collider;
    
    public bool slashActive { get; private set; }
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
                    creature.GetDamage(1);
                }
            }
            yield return null;
        }
    }
    
    public void SlashStart()
    {
        slashActive = true;
        _collider.enabled = true;
        StartCoroutine(DamageWhileSlash());
    }
    public void SlashStop()
    {
        _collider.enabled = false;
        slashActive = false;
    }
}
