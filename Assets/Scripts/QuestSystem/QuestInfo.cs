using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    [Tooltip("Set during playtime")] public Quest quest;
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image completionMark;
    [SerializeField] private Image followMark;

    public void Init(Quest newQuest)
    {
        quest = newQuest;
        title.text = quest.title;
        if (description != null) description.text = quest.description;
        if (followMark != null) followMark.gameObject.SetActive(newQuest.follow);
        UpdateProgress(newQuest.progress);
    }
    
    public void UpdateProgress(float progress)
    {
        progressBar.fillAmount = progress;
        if (completionMark != null)
        {
            if ((progress >= 1f)) completionMark.gameObject.SetActive(true);
            else completionMark.gameObject.SetActive(false);
        }
    }
    public void SetFollowQuest()
    {
        if (quest.follow) Player.Instance.questManager.RemoveFollowQuest();
        else Player.Instance.questManager.SetFollowQuest(quest.codeName);
    }
}
