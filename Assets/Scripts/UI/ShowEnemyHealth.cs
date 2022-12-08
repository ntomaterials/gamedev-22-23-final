using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowEnemyHealth : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Enemy target;
    
    [Header("Base")]
    [SerializeField] private Image healthbar;
    [SerializeField] private Image healhbarBG;

    private SpriteRenderer _targetRenderer; // for mages

    private void Start()
    {
        if (target == null)
        {
            target = transform.parent.GetComponent<Enemy>();
        }
        _targetRenderer = target.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        SetVisible(_targetRenderer.enabled);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y * 2, 0));
        healthbar.fillAmount = target.healthPercent;
    }

    
    private void SetVisible(bool visible)
    {
        healthbar.gameObject.SetActive(visible);
        healhbarBG.gameObject.SetActive(visible);
    }
}
