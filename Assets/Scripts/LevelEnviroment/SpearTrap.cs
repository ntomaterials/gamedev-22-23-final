using UnityEngine;
public class SpearTrap : ToActiveCycle
{
    [SerializeField] private int damage;

    [SerializeField] private float length;
    [SerializeField] private float velocity;
    [SerializeField] private float timeInside;
    [SerializeField] private float timeOutside;
    private float timeBtwAction;

    [SerializeField] private SpriteRenderer woodSprite;

    private Vector3 startPos;
    private Rigidbody2D rb;
    private float xEdgePoint;
    private bool isRight;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.localPosition;
        xEdgePoint = startPos.x + length / 2;
        woodSprite.size = new Vector2(length / 2, 0.5f);
        isRight = true;
    }
    private void Update()
    {
        if (isActive)
        {
            if (timeBtwAction <= 0)
            {
                if (isRight)
                {
                    MoveTo(new Vector2(xEdgePoint, startPos.y), Vector2.right, timeOutside);
                }
                else
                {
                    MoveTo(startPos, Vector2.left, timeInside);
                }
            }
            else timeBtwAction -= Time.deltaTime;
        }
    }
    private void MoveTo(Vector3 to, Vector2 dir, float timer)
    {
        if (transform.localPosition.x * dir.x < to.x * dir.x) 
        {
            transform.localPosition += Vector3.right * velocity * Time.deltaTime*dir.x;
        }
        else
        {
            transform.localPosition = to;
            timeBtwAction = timer;
            if (transform.localPosition.x <= startPos.x) isRight = true;
            else if (transform.localPosition.x >= xEdgePoint) isRight = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) Player.Instance.GetDamage(damage);
    }
}
