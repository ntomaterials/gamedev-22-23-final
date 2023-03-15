using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    [Tooltip("Set during playtime")] public Quest quest;
    [SerializeField] private Text title;
    [SerializeField] private Text description;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image completionMark;
    public void Init(Quest newQuest)
    {
        quest = newQuest;
        title.text = quest.title;
        description.text = quest.description;
    }
    public void Init(Quest newQuest, float progress)
    {
        Init(newQuest);
        UpdateProgress(progress);
    }
    public void UpdateProgress(float progress)
    {
        progressBar.fillAmount = progress;
        if ((progress >= 1f))completionMark.gameObject.SetActive(true);
        else completionMark.gameObject.SetActive(false);
    }
}
