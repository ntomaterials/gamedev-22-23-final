using System.Collections.Generic;
using UnityEngine;

public class QuestInfoManager : MonoBehaviour
{
    public static QuestInfoManager Instance;
    [SerializeField] private GameObject questInfoPrefab;
    private List<QuestInfo> _questInfos;
    private void Awake()
    {
        _questInfos = new List<QuestInfo>();
    }

    public void AddQuest(Quest newQuest)
    {
        GameObject questInfoGameObject = Instantiate(questInfoPrefab, this.transform);
        QuestInfo newQuestInfo = questInfoGameObject.GetComponent<QuestInfo>();
        newQuestInfo.Init(newQuest, newQuest.progress);
        _questInfos.Add(newQuestInfo);
    }
    public void UpdateInfos(List<Quest> quests)
    {
        foreach(QuestInfo questinfo in _questInfos)
        {
            Destroy(questinfo.gameObject);
        }
        _questInfos = new List<QuestInfo>();

        foreach (Quest quest in quests)
        {
            AddQuest(quest);
        }
    }
}