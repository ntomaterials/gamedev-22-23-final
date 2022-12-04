using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPlatform : MonoBehaviour // Фикс платформы, чтобы ее можно было удобно тайлить
{
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;
    private const float addSize = 1.2f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        float Xoffset = spriteRenderer.size.x / 2;
        Left.transform.localPosition = new Vector2(-Xoffset, 0);
        Right.transform.localPosition = new Vector2(Xoffset, 0);
        float Xsize = spriteRenderer.size.x+addSize;
        collider.size = new Vector2(Xsize, collider.size.y);
    }
}
