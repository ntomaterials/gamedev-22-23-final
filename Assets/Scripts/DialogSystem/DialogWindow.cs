using UnityEngine;
using UnityEngine.UI;
public class DialogWindow : MonoBehaviour
{
public static DialogWindow Instance;
public Text nameText;
public Text textArea;
[SerializeField] private GameObject answerPanel;
private void Awake()
{
    Instance=this;
    HideAnswerWindow();
}
public void ShowAnswerWindow()
{
answerPanel.SetActive(true);
}
public void HideAnswerWindow()
{
answerPanel.SetActive(false);    
}
}
