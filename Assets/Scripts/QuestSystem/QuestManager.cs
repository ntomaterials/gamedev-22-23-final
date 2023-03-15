using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public QuestInfoManager questInfoManager;
    private Animator _questInfoAnimator;
    public Quest followedQuest { get; private set; }
    private void Start()
    {
        _questInfoAnimator = questInfoManager.GetComponentInParent<Animator>();
        HideQuestMenu();
    }

    public void NewQuest(Quest quest)
    {
        quests.Add(quest);
        questInfoManager.UpdateInfos(quests);
    }

    public void UpdateQuest(string codeName, float progress)
    {
        followedQuest = null;
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
        questInfoManager.UpdateInfos(quests);
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
        _questInfoAnimator.SetBool("active", true);
        Time.timeScale = 0f;
        questInfoManager.UpdateInfos(quests);
    }
    public void HideQuestMenu()
    {
        _questInfoAnimator.SetBool("active", false);
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
                if (quests[i].follow) followedQuest = quests[i];
                followedQuest = quests[i];
            }
            else quests[i].follow = false;
        }
        questInfoManager.UpdateInfos(quests);
    }
    public void RemoveFollowQuest()
    {
        followedQuest = null;
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].follow = false;
        }
        questInfoManager.UpdateInfos(quests);
    }
    public void QuitQuest(string codeName)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].codeName == codeName)
            {
                if (quests[i].follow)
                {
                    followedQuest = null;
                }
                quests.RemoveAt(i);
                break;
            }
        }
        questInfoManager.UpdateInfos(quests);
    }
}
