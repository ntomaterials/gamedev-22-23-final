using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class CurseCleanerActivator : MonoBehaviour
{
    [SerializeField] private GameObject cleaner;
    [SerializeField] private LayerMask cleanerCantSpawnMask;
    [HideInInspector] public CurseCaster caster;
    private float radius;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        radius = caster.GetComponent<CircleCollider2D>().radius * caster.transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Creature creature = col.GetComponent<Creature>();
        if (creature == null) return;
        SpawnCleaner();
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Player.Instance.GetCursesStucksByType(caster.curseType) >= GlobalConstants.S)
        {
            _renderer.enabled = false;
        }
        else
        {
            _renderer.enabled = true;
        }
    }

    private void SpawnCleaner()
    {
        GameObject newCleaner = Instantiate(cleaner, Vector3.zero, Quaternion.identity);
        newCleaner.GetComponent<CurseCleaner>().caster = caster;
        Collider2D cleanerCol = newCleaner.GetComponent<Collider2D>();
        float r = Math.Max(cleanerCol.bounds.extents.x, cleanerCol.bounds.extents.y);
        Vector3 pos;
        do
        {
            pos = new Vector3((float)Random.Range(-radius, radius), (float)Random.Range(-radius, radius), 0) + caster.transform.position;
        } while (Physics2D.OverlapCircle(pos, r, cleanerCantSpawnMask));

        newCleaner.transform.position = pos;
    }
}