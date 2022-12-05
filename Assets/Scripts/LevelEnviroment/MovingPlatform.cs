using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class MovingPlatform : MonoBehaviour
{

    //[Header("Velocity")]
    [field: SerializeField] public float Xvelocity { get; private set; }
    [field: SerializeField] public float Yvelocity { get; private set; }

    //[Header("Cycle points")]
    [field: SerializeField] public float Xamplitude { get; private set; }
    [field: SerializeField] public float Yamplitude { get; private set; }

    public bool canMoving { get; protected set; }

    private float Xoffset;
    private float Yoffset;
    private float time;
    private Vector2 startPos;
    private Rigidbody2D rigidbody;
    protected void Awake()
    {
        startPos = transform.position;
        rigidbody = GetComponent<Rigidbody2D>();
        canMoving = true;
    }
    protected void Update() 
    {
        if (canMoving)
        {
            time = time + Time.deltaTime;
            if(Xamplitude!=0) Xoffset = Xamplitude * Mathf.Sin(time * Xvelocity/Xamplitude);
            if (Yamplitude!=0) Yoffset = Yamplitude * Mathf.Sin(time * Yvelocity/Yamplitude);
            transform.position = new Vector2(startPos.x + Xoffset, startPos.y + Yoffset);
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            collision.transform.parent = this.transform;
        }
    }
    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            collision.transform.parent=null;
        }
    }
    protected void OnDrawGizmos()
    {
        try
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Gizmos.color = Color.blue;
            float x =1.4f;
            float width = (Xamplitude+x*Xamplitude)+(x-Xamplitude)/x+(Xamplitude-x)/x;
            if (Xamplitude == 0) width = 0.2f;
            float height = Yamplitude * 2;//* renderer.size.y + renderer.size.y;
            if (Yamplitude == 0) height = 0.2f;
            Vector2 center = startPos;
            if (startPos == null || startPos == Vector2.zero) center = transform.position;
            Gizmos.DrawWireCube(center, new Vector3(width, height, 0));
            /*float x = (renderer.size.x + 1.88f);
            float width = x*Xamplitude/1.5f+x;
            if (Xamplitude == 0) width = 0.2f;
            float height = Yamplitude * 2;//* renderer.size.y + renderer.size.y;
            if (Yamplitude == 0) height = 0.2f;
            Vector2 center = startPos;
            if (startPos == null || startPos==Vector2.zero) center = transform.position;
            Gizmos.DrawWireCube(center, new Vector3(width, height, 0));*/
        }
        catch
        {
            return;
        }
    }
}
