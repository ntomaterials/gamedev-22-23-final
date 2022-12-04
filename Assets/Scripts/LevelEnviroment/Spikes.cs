using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Creature creature;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        creature = collision.gameObject.GetComponent<Creature>();
        if (creature!=null)
        {
            creature.Die();
            creature = null;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        creature = null;
    }
}
