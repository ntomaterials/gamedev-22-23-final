using UnityEngine;

public class Enemy : Creature
{
    [field: SerializeField] public int damage { get; private set; }
    [SerializeField] private LayerMask playerLayerMask = LayerMask.GetMask("Player");
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerLayerMask == (playerLayerMask | (1 << collision.gameObject.layer)))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.GetDamage(damage, -player.GetComponent<Rigidbody2D>().velocity);
        }
    }
}
