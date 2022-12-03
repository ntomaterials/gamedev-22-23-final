using UnityEngine;
using Random = UnityEngine.Random;

public class Mage : Enemy
{
    [SerializeField] private float teleportReload = 5f;
    [SerializeField] private float teleportTriggerRadius = 2f;
    [SerializeField] private CurseCaster curseCaster;
    [SerializeField] private LayerMask canNotSpawnIn;
    private float _teleportCooldown = 2f;
    
    private SpriteRenderer _renderer;

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        _teleportCooldown -= Time.fixedDeltaTime;
        if (Player.Instance.GetCursesStucksByType(curseCaster.curseType) >= CursesManager.Instance.S)
        {
            _renderer.enabled = false;
        }
        else
        {
            _renderer.enabled = true;
        }
        CheckForTeleport();
    }

    private void CheckForTeleport()
    {
        float distance = (Player.Instance.transform.position - transform.position).magnitude;

        if (distance <= teleportTriggerRadius && _teleportCooldown <= 0)
        { 
            Teleport();
        }
    }

    private void Teleport()
    {
        _teleportCooldown = teleportReload;
        int attempts = 0;
        int maxAttemps = 1000;
        Vector3 pos = Vector2.zero;
        float r = Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y);
        float tpRadius = curseCaster.radius * curseCaster.transform.localScale.x;
        Vector3 dir = Vector2.zero;
        Quaternion rot;
        float angle = 0;
        while (attempts++ <= maxAttemps)
        {
            dir = transform.position - Player.Instance.transform.position; 
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // угол в сторону игрока
            angle = angle + Random.Range(-60, 60); // получается рандомное направление в рамках противоположной от игрока полуокружности
            
            rot = Quaternion.Euler(0, 0, angle);
            dir = rot * dir;

            pos = transform.position + dir.normalized * tpRadius;
            if (!Physics2D.OverlapCircle(pos, 0.1f, canNotSpawnIn)) { break; }
            
        };
        if (attempts > maxAttemps) print("Out of attempts");
        
        attempts = attempts / 2;
        while (attempts++ <= maxAttemps) // повторно пробуем телепортироваться, на этот рас хоть в какую нибудь точку окружности(костыльно немного но вроде работает)
        {
            dir = transform.position - Player.Instance.transform.position; 
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // угол в сторону игрока
            angle = angle + Random.Range(-180, 180); // получается рандомное направление в рамках противоположной от игрока полуокружности
            
            rot = Quaternion.Euler(0, 0, angle);
            dir = rot * dir;

            pos = transform.position + dir.normalized * tpRadius * Random.Range(0.8f, 1.5f);
            if (!Physics2D.OverlapCircle(pos, r, canNotSpawnIn)) { break; }
            
        };

        transform.position = pos;
    }
}
