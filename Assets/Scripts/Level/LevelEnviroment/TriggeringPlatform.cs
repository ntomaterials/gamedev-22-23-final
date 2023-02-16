using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringPlatform : MovingPlatform
{
    [SerializeField] private Vector2 endPos;
    private void Awake()
    {
        base.Awake();
        canMoving = false;
    }
    private void Update()
    {
        base.Update();
        if (endPos.x!=0 && transform.position.x - endPos.x>=0) canMoving = false;
        if (endPos.y!=0 && transform.position.y - endPos.y >= 0) canMoving = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            if (!canMoving) canMoving = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
    }
}
