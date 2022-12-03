using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Collider2D))]
public class Sword : MonoBehaviour
{
    [SerializeField] private ContactFilter2D contactFilter;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackPower = 2f;
    [SerializeField] private float reload = 2f;
    
    public float blockReload=1f;
    public bool hasBlock; // на будущее
    
    private Collider2D _collider;
    
    public bool slashActive { get; private set; }
    private float _reloadTime = 0f;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _reloadTime -= Time.deltaTime;
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

    public bool ready
    {
        get
        {
            return _reloadTime <= 0;
        }
    }
    
    public void SlashStart()
    {
        slashActive = true;
        StartCoroutine(DamageWhileSlash());
    }
    public void SlashStop()
    {
        slashActive = false;
    }
}
