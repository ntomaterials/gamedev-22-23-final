using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum CurseType
{
    Fire, Water,
}
[RequireComponent(typeof(CircleCollider2D))]
public class CurseCaster : MonoBehaviour
{
    public CurseType curseType;
   
    [SerializeField] private bool castOnlyOnPlayer = true;
    [SerializeField] private VisualEffect curseSphere;
    [SerializeField] private int stacksPerCastMax = 5;
    [SerializeField] private GameObject curseCleaner;

    private List<Creature> _targets = new List<Creature>();
    private const float castSpeed = 1f;
    private bool _activatorSpawned;
    private Dictionary<Creature, GameObject> _curseRays = new Dictionary<Creature, GameObject>(); // ссылка на цель и на сам луч
    
    private CircleCollider2D _collider;

    private Enemy owner;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        owner = transform.parent.GetComponent<Enemy>();
        transform.parent = null; // fix rotation
    }

    private void LateUpdate()
    {
        if (owner == null)
        {
            Destroy(this.gameObject);
            return;
        }
        transform.position = owner.transform.position;
        UpdateVisualIntesity();
    }

    public float radius
    {
        get { return _collider.radius; }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (castOnlyOnPlayer)
        {
            if (col.GetComponent<Player>() == null) return;;
        }
        Creature creature = col.GetComponent<Creature>();
        if (creature != null && !_targets.Contains(creature))
        {
            if (!CheckIfInRadius(col.transform.position)) return;
            _targets.Add(creature);
            if (!_activatorSpawned)
            {
                _activatorSpawned = true;
                GameObject activator = Instantiate(curseCleaner, transform);
                // поворот в сторону противоположну от игрока
                Vector2 dir = col.bounds.center - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                activator.transform.localEulerAngles = new Vector3(0, 0, angle + 90f); // криво импортировл тестовый конус, поэтому +90)
                activator.GetComponent<CurseCleaner>().caster = this;
            }
            StartCoroutine(CastCurseOverTime(creature));
        }
    }

    private void UpdateVisualIntesity()
    {
        float distance = (Player.Instance.transform.position - transform.position).magnitude;
        if (distance > radius * 1.5f)
        {
            curseSphere.enabled = false;
            return;
        }
        else
        {
            curseSphere.enabled = true;
        }
        
        float inten = Mathf.Clamp(radius / distance - 0.5f, 0, 1.5f);
        curseSphere.SetFloat("Intensity", inten);
        if (Player.Instance.GetCursesStucksByType(curseType) >= CursesManager.Instance.S)
        {
            curseSphere.enabled = false;
        }
        else
        {
            curseSphere.enabled = true;
        }
    }


    private IEnumerator CastCurseOverTime(Creature target)
    {
        while (_targets.Contains(target))
        {
            float distance = (target.transform.position - transform.position).magnitude;
            if (!CheckIfInRadius(target.transform.position))
            {
                _targets.Remove(target);
                target.ClearCursesFromCaster(this);
                break;
            }
            int n = Mathf.Clamp(Mathf.CeilToInt((1 - distance / (_collider.radius * transform.localScale.x)) * stacksPerCastMax), 1, stacksPerCastMax);
            CastCurse(target, n);
            yield return new WaitForSeconds(castSpeed);
        }
    }


    private void CastCurse(Creature target, int stacks)
    {
        switch (curseType)
        {
            case CurseType.Fire: target.AttachCurse(new FireCurse(target, this, stacks));
                break;
            case CurseType.Water: target.AttachCurse(new WaterCurse(target, this, stacks));
                break;
        };
    }

    public void Clear()
    {
        foreach (Creature target in _targets)
        {
            target.ClearCursesFromCaster(this);
        }
        gameObject.SetActive(false);
    }

    private bool CheckIfInRadius(Vector3 pos)
    {
        float distance = (pos - transform.position).magnitude;
        return (distance <= _collider.radius * transform.localScale.x);
    }

    private void OnValidate()
    {
        if (transform.parent == null)
        {
            Debug.LogWarning($"Нужен родитель {gameObject.name}");
        }
    }
}
