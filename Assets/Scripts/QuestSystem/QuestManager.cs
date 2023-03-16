using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public QuestMenuManager questMenuManager;
    public Quest followedQuest
    {
        get {
            Quest quest=null;
            foreach(Quest i in quests)
            {
                if (i.follow)
                {
                    quest = i;
                    break;
                }
            }
            return quest;
        }
    }
    private void Start()
    {
        HideQuestMenu();
    }

    public void NewQuest(Quest quest)
    {
        quests.Add(quest);
        questMenuManager.UpdateLabels(quests);
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
                break;
            }
        }
        questMenuManager.UpdateLabels(quests);
    }
    /// <summary>
    ///  Check if quest already given and how many
    /// </summary>
    public int CheckQuest(string codeName)
    {
        int n=0;
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].codeName == codeName)
            {
                n++;
            }
        }
        return n;
    }
    public void CompleteQuest(int id)
    {
        Player.Instance.GetXp(quests[id].expirienceReward);
        quests[id].isCompleted = true;
        quests[id].questType = QuestType.Completed;
    }
    public void ShowQuestMenu()
    {
        questMenuManager.Show();
        Time.timeScale = 0f;
        questMenuManager.UpdateLabels(quests);
    }
    public void HideQuestMenu()
    {
        questMenuManager.Hide();
        Time.timeScale = 1f;
    }
    public void ChangeQuestMenuState()
    {
        if (Time.timeScale == 0f)
        {
            HideQuestMenu();
        }
        else
        {
           ShowQuestMenu();
        }
    }
    public void SetFollowQuest(string codeName)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].codeName == codeName) {
                quests[i].follow = true;
            }
            else quests[i].follow = false;
        }
        questMenuManager.UpdateLabels(quests);
    }
    public void RemoveFollowQuest()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].follow = false;
        }
        questMenuManager.UpdateLabels(quests);
    }
    public void QuitQuest(string codeName)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].codeName == codeName)
            {
                quests.RemoveAt(i);
                break;
            }
        }
        questMenuManager.UpdateLabels(quests);
    }
    public void UpdateMenu()
    {
        questMenuManager.UpdateLabels(quests);
    }
}
