using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(CircleCollider2D))]
public class CurseCaster : MonoBehaviour
{
    public CurseType curseType;
   
    [SerializeField] private bool castOnlyOnPlayer = true;
    [SerializeField] private VisualEffect curseSphere;
    [SerializeField] private GameObject curseRayPrefab;
    [SerializeField] private int stacksPerCastMax = 5;
    [SerializeField] private GameObject curseCleaner;
    [SerializeField] float castSpeed = 1f;

    private List<Creature> _targets = new List<Creature>();
    private GameObject _activator;
    private Dictionary<Creature, GameObject> _curseRays = new Dictionary<Creature, GameObject>(); // ссылка на цель и на сам луч
    
    private CircleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y * 2, 0)); // при развороте глоб. ротация не меняется
        UpdateVisualIntesity();
        UpdateRays();
        if (!CheckIfInRadius(Player.Instance.transform.position))
        {
            Destroy(_activator);
        }else if (CheckIfInRadius(Player.Instance.transform.position))
        {
            if (_activator == null)
            {
                SpawnCleaner();
            }
        }
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
            if (_activator == null)
            {
                SpawnCleaner();
            }
            StartCoroutine(CastCurseOverTime(creature));
        }
    }

    private void SpawnCleaner()
    {
        _activator = Instantiate(curseCleaner, transform);
        // поворот в сторону противоположну от игрока
        Vector2 dir = Player.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _activator.transform.localEulerAngles = new Vector3(0, 0, angle + 90f); // криво импортировл тестовый конус, поэтому +90)
        _activator.GetComponent<CurseCleaner>().caster = this;
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
    private void UpdateRays()
    {
        if (!curseSphere.enabled)
        {
            foreach (GameObject ray in _curseRays.Values)
            {
                ray.SetActive(false);
            }
            return;
        }
        else
        {
            foreach (GameObject ray in _curseRays.Values)
            {
                ray.SetActive(true);
            }
        }
        foreach (Creature target in _targets)
        {
            if (!_curseRays.ContainsKey(target))
            {
                _curseRays[target] = Instantiate(curseRayPrefab, this.transform);
            }
        }

        List<Creature> toRemove = new List<Creature>();
        foreach (Creature target in _curseRays.Keys)
        {
            if (!_targets.Contains(target))
            {
                toRemove.Add(target);
            }
        }

        foreach (var target in toRemove)
        {
            Destroy(_curseRays[target]);
            _curseRays.Remove(target);
        }

        foreach (var ray in _curseRays)
        {
            Vector2 dir = transform.position - ray.Key.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            ray.Value.transform.eulerAngles = new Vector3(0, 0, angle + 90f);
            ray.Value.transform.localScale = new Vector3(1, dir.magnitude, 1) / transform.localScale.x;
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
            yield return new WaitForSeconds(1 / castSpeed);
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
            case CurseType.Darkness: target.AttachCurse(new DarknessCurse(target, this, stacks));
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
