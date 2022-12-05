using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTeleportReload : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Mage target;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.parent.rotation.eulerAngles.y * 2, 0));
        text.text = (Mathf.Round(target.teleportCooldown * 10) / 10).ToString();
        if (target.teleportCooldown <= 0 || !target.visiable)
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
