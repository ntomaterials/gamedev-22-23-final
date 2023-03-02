using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private bool useAttackingLayerInt=false;
    [SerializeField] private int attackingLayer;//для проверки имунитета
    public int damage = 1;
    [SerializeField] private float knockbackPower = 2f;
    public float speed = 5f;

    protected Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * speed;// new Vector3(0, _rigidbody.velocity.y) + transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
            Creature cr = col.collider.GetComponent<Creature>();
            if (cr)
            {
                Vector2 knock = ((rigidbody.velocity.x * Vector2.right).normalized + Vector2.up * 0.5f) * knockbackPower;
            if (useAttackingLayerInt && attackLayerMask == (attackLayerMask | (1 << col.gameObject.layer)))
                cr.GetDamage(damage, attackingLayer, knock);
            else
                cr.GetDamage(damage, knock);
            }
        Destroy(this.gameObject);
    }
    
}
