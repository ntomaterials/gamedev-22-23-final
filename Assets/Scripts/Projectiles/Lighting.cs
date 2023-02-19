using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Collider2D))]
public class Lighting : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private bool infinityLifetime = false;
    [SerializeField] private float stunDuration = 0.5f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float damagePerSecond=0.5f;
    [SerializeField] private LayerMask attackLayerMask;
    private float _lastDamageTimeLeft = 0f;

    private void Awake()
    {
        if (!infinityLifetime)
        {
            Destroy(this.gameObject, lifetime);
        }
        
    }

    private void Update()
    {
        _lastDamageTimeLeft -= Time.deltaTime;
    }
    

    private void OnTriggerStay2D(Collider2D col)
    {
        if (_lastDamageTimeLeft > 0) return;
        if (attackLayerMask == (attackLayerMask | (1 << col.gameObject.layer)))
        {
            Player cr = col.GetComponent<Player>();
            if (cr)
            {
                cr.GetDamage(damage);
                cr.Stun(stunDuration);
                _lastDamageTimeLeft = 1 / damagePerSecond;
            }
        }
    }
}
