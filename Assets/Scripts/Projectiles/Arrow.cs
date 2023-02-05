using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayerMask;
    public int damage = 1;
    [SerializeField] private float knockbackPower = 2f;
    public float speed = 5f;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = transform.right * speed;// new Vector3(0, _rigidbody.velocity.y) + transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (attackLayerMask == (attackLayerMask | (1 << col.gameObject.layer)))
        {
            Creature cr = col.collider.GetComponent<Creature>();
            if (cr)
            {
                Vector2 knock = ((_rigidbody.velocity.x * Vector2.right).normalized + Vector2.up * 0.5f) * knockbackPower;
                cr.GetDamage(damage, knock);
            }
        }
        Destroy(this.gameObject);
    }
    
}
