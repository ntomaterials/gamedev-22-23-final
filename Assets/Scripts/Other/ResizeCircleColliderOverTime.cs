using System;
using UnityEngine;

public class ResizeCircleColliderOverTime : MonoBehaviour
{
    [SerializeField] private CircleCollider2D collider;
    [SerializeField] private float radiusChangePerSecond = 0.1f;

    private void FixedUpdate()
    {
        collider.radius = collider.radius + radiusChangePerSecond * Time.fixedDeltaTime;
    }
}
