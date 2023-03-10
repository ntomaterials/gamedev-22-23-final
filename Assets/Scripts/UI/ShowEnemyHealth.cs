using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowEnemyHealth : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Creature target;

    [Space(5)]
    [SerializeField] [Tooltip("Если включено, hp бар будет перемещатся за целью")]private bool targetIsNotParent = false;

    [SerializeField] private Vector3 offset;
    
    [Space(5)][Header("Base")]
    [SerializeField] private Image healthbar;
    [SerializeField] private Image healhbarBG;

    private SpriteRenderer _targetRenderer; // for mages
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (target == null)
        {
            target = transform.parent.GetComponent<Enemy>();
        }
        _targetRenderer = target.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (target == null) Destroy(this.gameObject);
        SetVisible(_targetRenderer.enabled);
        if (!targetIsNotParent)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y * 2, 0));
        }
        else
        {
            transform.position = target.transform.position + offset;
        }
        
        healthbar.fillAmount = target.healthPercent;
    }

    
    private void SetVisible(bool visible)
    {
        healthbar.gameObject.SetActive(visible);
        healhbarBG.gameObject.SetActive(visible);
    }
}
