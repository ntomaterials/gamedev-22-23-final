using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private Collider2D _collider;
    private bool canUse;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        canUse = false;
        InputHandler.Instance.onActionBtnUp += Port;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) canUse = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) canUse = false;
    }
    public void Port()
    {
        if (canUse)
        {
            Player.Instance.transform.position = _spawnPoint.position;
        }
    }
}