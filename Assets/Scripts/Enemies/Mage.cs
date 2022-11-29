using UnityEngine;
using Random = UnityEngine.Random;

public class Mage : Enemy
{
    [SerializeField] private float teleportReload = 5f;
    [SerializeField] private float teleportTriggerRadius = 2f;
    [SerializeField] private CurseCaster curseCaster;
    [SerializeField] private LayerMask canNotSpawnIn;
    private float _teleportCooldown = 2f;

    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        _teleportCooldown -= Time.fixedDeltaTime;
        if (Player.Instance.GetCursesStucksByType(curseCaster.curseType) > GlobalConstants.S)
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
        float r = Mathf.Max(_collider.bounds.extents.x, _collider.bounds.extents.y);
        float tpRadius = curseCaster.radius * curseCaster.transform.localScale.x;
        Vector3 dir = Vector2.zero;
        Quaternion rot;
        float angle = 0;
        while (attempts++ <= maxAttemps)
        {
            dir = transform.position - Player.Instance.transform.position; 
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // угол в сторону игрока
            angle = angle + Random.Range(-90, 90); // получается рандомное направление в рамках противоположной от игрока полуокружности
            
            rot = Quaternion.Euler(0, 0, angle);
            dir = rot * dir;

            pos = transform.position + dir.normalized * tpRadius;
            if (!Physics2D.OverlapCircle(pos, r, canNotSpawnIn)) { break; }
            
        };
        if (attempts > maxAttemps) print("Out of attempts");

        transform.position = pos;
    }
}
