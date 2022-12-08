using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMarker : MonoBehaviour
{
    public int Score;
    [SerializeField] private float teleportRadius = 4f;
    private bool canUse;
    private Collider2D collider;
    //[SerializeField] private LayerMask canNotSpawnIn;
    private InputHandler inputHandler;
    private Player player;
    //private Rigidbody2D rigidbody;
    private void Awake()
    {
        canUse = false;
        player = FindObjectOfType<Player>();
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onActionBtnUp += GetXp;
        collider = GetComponent<Collider2D>();
        //rigidbody = GetComponent<Rigidbody2D>();
        //rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
    public void GetXp()
    {
        player.GetXp(Score);
        canUse = false;
        Destroy(gameObject);
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canUse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canUse = false;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision) // Надо потом написать метод, который чекает, где можно заспавнить магилу
                                                           // (если заспавнится на шипах, будет не круто)
    {
        if (!Physics2D.OverlapCircle(transform.position, 0.1f, canNotSpawnIn)) { Teleport(); }
    }
    private void Teleport()
    {
        int attempts = 0;
        int maxAttemps = 1000;
        Vector3 pos = Vector2.zero;
        float r = Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y);
        Vector3 dir = Vector2.zero;
        Quaternion rot;
        float angle = 0;
        while (attempts++ <= maxAttemps)
        {
            dir = (transform.position - Player.Instance.transform.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x); // угол в сторону игрока
            angle = angle + Random.Range(-60, 60) * Mathf.Deg2Rad; // получается рандомное направление в рамках противоположной от игрока полуокружности
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            dir = new Vector2(x, y);
            pos = transform.position + dir * teleportRadius;
            teleportRadius += 0.1f;
            if (!Physics2D.OverlapCircle(pos, 0.1f, canNotSpawnIn)) { break; }

        };
        if (attempts > maxAttemps) print("Out of attempts");
        transform.position = pos;
    }*/
}
