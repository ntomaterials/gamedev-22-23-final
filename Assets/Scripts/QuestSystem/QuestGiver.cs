using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public void GiveQuest()
    {
        Player.Instance.questManager.NewQuest(quest);
    }
}
