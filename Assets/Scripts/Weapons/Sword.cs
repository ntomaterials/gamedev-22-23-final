using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Sword : Weapon
{
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private Vector2 knockbackPower;

    private Collider2D _collider;
    [SerializeField] private List<AudioClip> attackSounds;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
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
                    creature.GetDamage(damage, new Vector2(transform.right.x, 0.5f) * knockbackPower); // направление отдачи - это направление меча
                }
            }
            yield return null;
        }
    }
    
    
    public override void SlashStart()
    {
        if (attackSounds.Count > 0)
        {
            try
            {
                int i = Random.Range(0, attackSounds.Count);
                audioSource.PlayOneShot(attackSounds[i]);
            }
            catch {  }
        }
        slashActive = true;
        StartCoroutine(DamageWhileSlash());
    }

    public void SingleAttack()
    {
        List<Collider2D> hitColliders = new List<Collider2D>();
        List<Creature> hittedCreatures = new List<Creature>();
        Physics2D.OverlapCollider(_collider, contactFilter, hitColliders);

        foreach (var col in hitColliders)
        {
            Creature creature = col.GetComponent<Creature>();
            if (creature != null && !hittedCreatures.Contains(creature))
            {
                hittedCreatures.Add(creature);
                if (creature.effectedByKnockback)
                {
                    creature.GetDamage(damage, new Vector2(transform.right.x, 0.5f) * knockbackPower); // направление отдачи - это направление меча
                }
                else
                {
                    creature.GetDamage(damage, Vector2.zero);
                }
                
            }
        }
        
    }
    public override void SlashStop()
    {
        slashActive = false;
    }
}
