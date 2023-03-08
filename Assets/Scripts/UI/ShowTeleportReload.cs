using TMPro;
using UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowTeleportReload : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    private IReloadable target;

    private void Awake()
    {
        target = transform.parent.GetComponent<IReloadable>();
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y * 2, 0));
        text.text = (Mathf.Round(target.cooldown * 10) / 10).ToString();
        if (target.cooldown <= 0)
        {
            SetVisible(false);
        }
        else
        {
            SetVisible(true);
        }
    }

    private void SetVisible(bool visible)
    {
        text.gameObject.SetActive(visible);
        image.gameObject.SetActive(visible);
    }
}
