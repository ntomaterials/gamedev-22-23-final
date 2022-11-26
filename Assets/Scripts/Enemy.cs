using UnityEngine;

public class Enemy : Creature
{
    [field: SerializeField] public int damage { get; private set; }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (GlobalConstants.PlayerLayer == collision.gameObject.layer)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Vector2 dir;
            if (rb.velocity != Vector2.zero) 
            {
                if (player.transform.position.x >= transform.position.x) dir = new Vector2(1, 1);
                else dir = new Vector2(-1, 1);
            }
            else dir = new Vector2(transform.right.x, 1);
            player.GetDamage(damage, dir);
        }
    }
}
