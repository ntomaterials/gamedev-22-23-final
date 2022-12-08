using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class XpText : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private float updateTime;
    [SerializeField] private float updateSpeedCoof;
    private Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.onXpChanged += TextUpdate;
    }
    public void TextUpdate(int value)
    {
        StartCoroutine(TextUpd(value));
    }
    private IEnumerator TextUpd(int value)
    {
        int step = (int)(Mathf.Round(value / updateTime)); //- value * updateSpeedCoof; // Чем больше у нас пришло опыта, тем дольше мы его зачисляем.
                                                                   // Первое слагаемое - фиксирование времени, второе - уменьшение шага
        string sign = "+";
        if (step < 0) sign = "-";
        for (float i = 0; Mathf.Abs(i)<Mathf.Abs(value); i += step)
        {
            text.text = sign+i.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        text.text = player.playerXp.ToString();
    }
}
