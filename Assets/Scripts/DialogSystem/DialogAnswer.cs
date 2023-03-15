using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogAnswer : MonoBehaviour
{
    
    public Dialog playerAnswer;
    public Dialog nextDialog;
    [SerializeField] private string answer;
    [SerializeField] private Text answerText;
    private DialogManager dialogManager;

    private void Start() 
    {
        dialogManager=DialogManager.Instance;
        answerText.text=answer;

        playerAnswer.npcName=GlobalConstants.PlayerName;
    }
    public void StartNextDialog()
    {
    dialogManager.StartDialog(playerAnswer, nextDialog);
    }
    public void EndReplic()
    {
        dialogManager.EndDialog();
    }
}
