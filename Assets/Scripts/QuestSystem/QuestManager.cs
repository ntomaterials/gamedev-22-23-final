using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;

    public void NewQuest(Quest quest)
    {
        quests.Add(quest);
    }
    public void RemoveQuestByCodeName(string codeName)
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
}
