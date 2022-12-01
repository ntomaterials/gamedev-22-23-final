using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedWater : Water
{
    [SerializeField] private int damage;
    [SerializeField] private float cooldown;
    private Coroutine routine;
    private Creature creature;
    private void Awake()
    {
        base.Awake();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);

        if(creature==null)creature = collision.GetComponent<Creature>();
        if(creature!=null) StartPoison(creature);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopPoison();
        creature = null;
    }
    private IEnumerator Poison(Creature creature)
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            creature.GetDamage(damage);
        }
    }
    // Цыганские фокусы с корутиной. Так не должны накладываться кучи эффектов
    private void StartPoison(Creature creature)
    {
        if (routine == null) routine = StartCoroutine(Poison(creature));
    }
    private void StopPoison()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }
}
