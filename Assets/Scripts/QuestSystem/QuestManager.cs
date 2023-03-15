using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests;
    public QuestInfoManager questInfoManager;
    private Animator _questInfoAnimator;
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
                //questInfoManager.UpdateInfos(quests);
                break;
            }
        }
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

}
