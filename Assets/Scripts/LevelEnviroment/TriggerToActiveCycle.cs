using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class TriggerToActiveCycle : MonoBehaviour
{
    [SerializeField] private List<ToActiveCycle> objs;
    private BoxCollider2D collider;
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            foreach (ToActiveCycle obj in objs)
            {
                obj.isActive = true;
                obj.StartDo();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            foreach (ToActiveCycle obj in objs)
            {
                obj.isActive = false;
                obj.StopDo();
            }
        }
    }
}
