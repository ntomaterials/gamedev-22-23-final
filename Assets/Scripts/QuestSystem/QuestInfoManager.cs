using System.Collections.Generic;
using UnityEngine;

public class QuestInfoManager : MonoBehaviour
{
    public static QuestInfoManager Instance;
    [SerializeField] private GameObject questInfoPrefab;
    [SerializeField] private Transform gridLayaout;
    [SerializeField] private QuestInfo followQuestInfo;
    private List<QuestInfo> _questInfos;
    private void Awake()
    {
        _questInfos = new List<QuestInfo>();
    }

    public void AddQuest(Quest newQuest)
    {
        GameObject questInfoGameObject = Instantiate(questInfoPrefab, gridLayaout);
        QuestInfo newQuestInfo = questInfoGameObject.GetComponent<QuestInfo>();
        newQuestInfo.Init(newQuest);
        _questInfos.Add(newQuestInfo);
    }
    public void UpdateInfos(List<Quest> quests)
    {
        RemoveFollowQuest();
        foreach (QuestInfo questinfo in _questInfos)
        {
            Destroy(questinfo.gameObject);
        }
        _questInfos = new List<QuestInfo>();

        foreach (Quest quest in quests)
        {
            AddQuest(quest);
            if (quest.follow)
            {
                followQuestInfo.gameObject.SetActive(true);
                followQuestInfo.Init(quest);
            }
        }

    }
    public void RemoveFollowQuest()
    {
        followQuestInfo.gameObject.SetActive(false);
    }
}