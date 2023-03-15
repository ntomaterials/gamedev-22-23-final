using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public QuestInfoManager questInfoManager;

    public void NewQuest(Quest quest)
    {
        quests.Add(quest);
        questInfoManager.UpdateInfos(quests);
    }
    /// <summary>
    /// Пожалуй лучше не удалять квесты но пусть будет
    /// </summary>
    public void RemoveQuest(string codeName)
    {
        int id=-1;
        for(int i=0; i < quests.Count; i++)
        {
            if (quests[i].codeName == codeName)
            {
                id = i;
                break;
            }
        }
        if (id != -1)
        {
            quests.RemoveAt(id);
        }
    }
    public void UpdateQuest(string codeName, float progress)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].codeName == codeName)
            {
                quests[i].progress += progress;
                if (quests[i].progress >= 1f)
                {
                    CompleteQuest(i);
                }
                questInfoManager.UpdateInfos(quests);
                break;
            }
        }
    }
    public void CompleteQuest(int id)
    {
        Player.Instance.GetXp(quests[id].expirienceReward);
        quests[id].isCompleted = true;
    }
}
