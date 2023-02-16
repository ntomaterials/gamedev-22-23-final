using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Water : MonoBehaviour
{
    [SerializeField] private float forceCoof;
    private Rigidbody2D obj;
    private Collider2D collider;
    //[SerializeField] private float forceLimit;
    protected void Awake()
    {
        collider = GetComponent<Collider2D>();
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        obj = collision.GetComponent<Rigidbody2D>();
        if (obj == null) return;

        float difference = Mathf.Abs(obj.transform.position.y - (transform.position.y - transform.localScale.y/2)) * forceCoof+0.01f;
        Vector2 arhForce = new Vector2(obj.velocity.x, Physics2D.gravity.y * difference);//Mathf.Clamp(Physics2D.gravity.y * difference, Physics2D.gravity.y, 0));
        obj.velocity = arhForce;
    }

    ///Физика для выталкивания объектов. Под замедление не настраивается

    /*
    protected void OnTriggerStay2D(Collider2D collision)
    {
        obj = collision.GetComponent<Rigidbody2D>();
        if (obj == null) return;

        float difference = (obj.transform.position.y - transform.position.y+(collider.bounds.extents.y * Vector2.up).y)*forceCoof;
        Vector2 arhForce = new Vector2(0, Mathf.Clamp((Mathf.Abs(Physics2D.gravity.y)*difference), 0, (Mathf.Abs(Physics2D.gravity.y)*forceLimit)));
        obj.AddForce(arhForce, ForceMode2D.Impulse );
    }*/
}
