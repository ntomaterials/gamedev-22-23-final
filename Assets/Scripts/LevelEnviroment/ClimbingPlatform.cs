using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPlatform : MonoBehaviour // Фикс платформы, чтобы ее можно было удобно тайлить
{
    [SerializeField] private Transform Left;
    [SerializeField] private Transform Right;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;
    [SerializeField] private float addCollSize = 1.2f;
    [SerializeField] private float addLRsize;
    [SerializeField] private float Yoffset;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        float Xoffset = addLRsize*(spriteRenderer.size.x+1);
        Left.transform.localPosition = new Vector2(-Xoffset, Yoffset);
        Right.transform.localPosition = new Vector2(Xoffset, Yoffset);
        float Xsize = spriteRenderer.size.x+addCollSize;
        collider.size = new Vector2(Xsize, collider.size.y);
    }
}
