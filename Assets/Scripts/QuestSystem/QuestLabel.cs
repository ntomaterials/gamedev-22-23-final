using UnityEngine;
using UnityEngine.UI;
public class QuestLabel : MonoBehaviour
{
    public Quest quest;
    [SerializeField] private Text title;

    public void Init(Quest newQuest)
    {
        quest = newQuest;
        title.text = quest.title;
    }
    public void ShowInfo()
    {
        Player.Instance.questManager.questMenuManager.ShowQuestInfo(quest);
    }
}
