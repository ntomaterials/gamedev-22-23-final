using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnswerBtn : MonoBehaviour
{
    public DialogAnswer answer;
        [SerializeField] private Text answerText;
    private DialogManager dialogManager;
    private QuestManager questManager;

    private void Start() 
    {
        dialogManager=DialogManager.Instance;
        questManager=Player.Instance.questManager;
        answerText.text=answer.answerText;

        answer.playerAnswer.npcName=GlobalConstants.PlayerName;
    }
    public void Answer()
    {
        if(answer.questGiver!=null)
        {
            if(answer.questGiver.quest.codeName!="")
            {
                Quest quest=answer.questGiver.quest;
            if(questManager.CheckQuest(quest.codeName)==0)
            {
                answer.questGiver.GiveQuest();
            }
            }
        }
        if(answer.IsEndReplic) dialogManager.EndDialog();
        else dialogManager.StartDialog(answer.playerAnswer, answer.nextDialog);
    }
}
