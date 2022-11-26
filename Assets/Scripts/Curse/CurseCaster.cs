using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurseType
{
    Fire, Water,
}
[RequireComponent(typeof(CircleCollider2D))]
public class CurseCaster : MonoBehaviour
{
    [SerializeField] private CurseType curseType;
    [SerializeField] private int stacksPerCastMax = 5;
    [SerializeField] private GameObject curseCleanerActivator;
    private CircleCollider2D _collider;
    private List<Creature> _targets = new List<Creature>();
    private const float castSpeed = 1f;
    private bool _activatorSpawned;
    private float _teleportReloadTick = 0; // если меньше нуля то активен телепорт
    
    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        Creature creature = col.GetComponent<Creature>();
        if (creature != null && !_targets.Contains(creature))
        {
            if (!CheckIfInRadius(col.transform.position)) return;
            _targets.Add(creature);
            if (!_activatorSpawned)
            {
                _activatorSpawned = true;
                GameObject activator = Instantiate(curseCleanerActivator, transform);
                Vector2 dir = col.bounds.center - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                activator.transform.localEulerAngles = new Vector3(0, 0, angle + 90f); // криво импортировл тестовый конус, поэтому +90)
                activator.GetComponent<CurseCleanerActivator>().caster = this;
            }
            StartCoroutine(CastCurseOverTime(creature));
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
}
