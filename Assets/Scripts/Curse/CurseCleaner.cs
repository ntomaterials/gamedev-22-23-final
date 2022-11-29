using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CurseCleaner : MonoBehaviour
{
    [HideInInspector] public CurseCaster caster;
    private Collider2D _colider;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>() == null) return;
        caster.Clear();
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
}
