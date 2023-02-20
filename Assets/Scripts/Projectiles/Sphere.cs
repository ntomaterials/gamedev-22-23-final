           
using UnityEngine;

public class Sphere : Creature
{
    [SerializeField] private LayerMask attackLayerMask;
    [SerializeField] private GameObject dieFlash;
    public int damage = 1;
    [SerializeField] private float knockbackPower = 2f;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (attackLayerMask == (attackLayerMask | (1 << col.gameObject.layer)))
        {
            Creature cr = col.GetComponent<Creature>();
            if (cr)
            {
                Vector2 knock = ((rigidbody.velocity.x * Vector2.right).normalized + Vector2.up * 0.5f) * knockbackPower;
                cr.GetDamage(damage, knock);
            }
            else
            {
                Destroy(col.gameObject);
            }
            Die();
        }
    }

    public override void Die(){
        if (dieFlash != null)
        {
            GameObject flash = Instantiate(dieFlash, transform.position, dieFlash.transform.rotation);
            Destroy(flash, 0.2f);
        }
        this.gameObject.SetActive(false);
    }
}
