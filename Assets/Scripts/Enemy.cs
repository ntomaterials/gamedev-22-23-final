using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [field: SerializeField] public int damage { get; private set; }
    [SerializeField] private LayerMask playerLayerMask;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == playerLayerMask)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.GetDamage(damage, -player.GetComponent<Rigidbody2D>().velocity);
            player.HpBarUpdate();
        }
    }
}
