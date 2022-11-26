using UnityEngine;

public class Enemy : Creature
{
    [field: SerializeField] public int damage { get; private set; }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (GlobalConstants.PlayerLayer == collision.gameObject.layer)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.GetDamage(damage, -player.GetComponent<Rigidbody2D>().velocity);
        }
    }
}
