﻿using System;
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
    public CurseType curseType;
    [SerializeField] private bool castOnlyOnPlayer = true;
    [SerializeField] private GameObject curseRay;
    [SerializeField] private int stacksPerCastMax = 5;
    [SerializeField] private GameObject curseCleanerActivator;
    private CircleCollider2D _collider;
    private List<Creature> _targets = new List<Creature>();
    private const float castSpeed = 1f;
    private bool _activatorSpawned;
    private Dictionary<Creature, GameObject> _curseRays = new Dictionary<Creature, GameObject>();

    private SpriteRenderer _renderer;
    private Enemy _enemyCaster;

    private void Awake()
    {
        _enemyCaster = transform.parent.GetComponent<Enemy>();
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
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
                GameObject activator = Instantiate(curseCleanerActivator, transform);
                // поворот в сторону противоположну от игрока
                Vector2 dir = col.bounds.center - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                activator.transform.localEulerAngles = new Vector3(0, 0, angle + 90f); // криво импортировл тестовый конус, поэтому +90)
                activator.GetComponent<CurseCleanerActivator>().caster = this;
            }
            StartCoroutine(CastCurseOverTime(creature));
        }
    }

    private void Update()
    {
        if (Player.Instance.GetCursesStucksByType(curseType) >= GlobalConstants.S)
        {
            _renderer.enabled = false;
        }
        else
        {
            _renderer.enabled = true;
        }
        UpdateRays();
    }

    private void UpdateRays()
    {
        foreach (Creature target in _targets)
        {
            if (!_curseRays.ContainsKey(target))
            {
                _curseRays[target] = Instantiate(curseRay, this.transform);
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