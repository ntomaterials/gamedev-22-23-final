using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Light2D))]
public class Product : MonoBehaviour
{
    [SerializeField] protected Collider2D triggerCollider;
    protected Light2D light2d;
    protected const float defaultIntens = 0.25f;
    protected const float activeIntens = 0.6f;

    protected bool canTake;
    protected PlayerInventory myInventory;
    protected virtual void Start()
    {
        light2d = GetComponent<Light2D>();
        triggerCollider.isTrigger = true;
        ChangeLightIntens(defaultIntens);

        canTake = false;
        InputHandler.Instance.onActionBtnUp += TakeItem;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) DoOnEnter();
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) DoOnExit();
    }
    protected virtual void DoOnEnter()
    {
        ChangeLightIntens(activeIntens);
        canTake = true;
    }
    protected virtual void DoOnExit()
    {
        ChangeLightIntens(defaultIntens);
        canTake = false;
    }
    public void TakeItem()
    {
        if (canTake)
        {
            if (myInventory.CanAdd())
            {
                myInventory.AddProduct(new Meet());
                Destroy(gameObject);
            }
        }
    }
    public virtual void OnUse() { }
    protected virtual void ChangeLightIntens(float intens)
    {
        light2d.intensity = intens;
    }
}
