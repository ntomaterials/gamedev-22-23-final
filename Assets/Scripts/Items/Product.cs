using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Light2D))]
public class Product : MonoBehaviour
{
    [SerializeField] protected Collider2D triggerCollider;
    protected Light2D light2d;
    protected const float defaultIntens = 0.25f;
    protected const float activeIntens = 0.6f;
    protected virtual void Start()
    {
        light2d = GetComponent<Light2D>();
        triggerCollider.isTrigger = true;
        ChangeLightIntens(defaultIntens);
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
    }
    protected virtual void DoOnExit()
    {
        ChangeLightIntens(defaultIntens);
    }
    protected virtual void ChangeLightIntens(float intens)
    {
        light2d.intensity = intens;
    }
}
