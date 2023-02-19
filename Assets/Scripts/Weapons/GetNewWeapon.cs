using UnityEngine;
public class GetNewWeapon : MonoBehaviour
{
    [SerializeField] private int weaponId;
    private bool canUse;
    private void Start()
    {
        canUse = false;
        InputHandler.Instance.onActionBtnUp += GetWeapon;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canUse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canUse = false;
        }
    }
    public void GetWeapon()
    {
        if (canUse)
        {
            Player.Instance.ActiveNewWeapon(2);
            Destroy(gameObject);
        }
    }
}
