using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnswerBtn : MonoBehaviour
{
    public DialogAnswer answer;
        [SerializeField] private Text answerText;
    private DialogManager dialogManager;

    private void Start() 
    {
        dialogManager=DialogManager.Instance;
        answerText.text=answer.answerText;

        answer.playerAnswer.npcName=GlobalConstants.PlayerName;
    }
    public void Answer()
    {
        if(answer.IsEndReplic) dialogManager.EndDialog();
        else dialogManager.StartDialog(answer.playerAnswer, answer.nextDialog);
    }
    /*public void Answer(GetQuest)
    {
        if(answer.IsEndReplic) dialogManager.EndDialog();
        else dialogManager.StartDialog(answer.playerAnswer, answer.nextDialog);
        GetQuest()
    }*/
}
