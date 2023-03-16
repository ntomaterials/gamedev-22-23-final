using System.Collections.Generic;
using UnityEngine;

public class QuestMenuManager : MonoBehaviour
{
    public static QuestMenuManager Instance;
    [SerializeField] private GameObject questLabelPrefab;
    [SerializeField] private Transform gridLayaout;
    [SerializeField] private int labelsInPage = 6;
    [SerializeField] private QuestInfo followQuestInfo;
    [SerializeField] private QuestInfo selectedQuestInfo;
    private List<QuestLabel> _questLabels;
    private QuestType currentQuestType;
    private Animator _anim;
    private Quest currentShownedQuest;
    private int _currentPage=0;
    private void Awake()
    {
        _questLabels = new List<QuestLabel>();
        _anim = GetComponentInParent<Animator>();
    }

    public void CreateQuestLabel(Quest newQuest)
    {
        GameObject questInfoGameObject = Instantiate(questLabelPrefab, gridLayaout);
        QuestLabel label = questInfoGameObject.GetComponent<QuestLabel>();
        label.Init(newQuest);
        _questLabels.Add(label);
    }
    public void Show()
    {
        currentShownedQuest = null;
        _anim.SetBool("active", true);
    }
    public void Hide()
    {
        selectedQuestInfo.gameObject.SetActive(false);
        _anim.SetBool("active", false);
    }
    public void UpdateLabels(List<Quest> quests)
    {
        RemoveFollowQuest();
        if (!quests.Contains(currentShownedQuest)) { 
            currentShownedQuest = null;
            selectedQuestInfo.gameObject.SetActive(false);
        }
        foreach (QuestLabel questinfo in _questLabels)
        {
            Destroy(questinfo.gameObject);
        }
        _questLabels = new List<QuestLabel>();
        int n = 0;
        foreach (Quest quest in quests)
        {
            if (quest.questType == currentQuestType)
            {
                if ((n >= labelsInPage * _currentPage && n < (_currentPage + 1) * labelsInPage))
                {
                    CreateQuestLabel(quest);
                }
                n++;
            }
            if (quest.follow)
            {
                followQuestInfo.gameObject.SetActive(true);
                followQuestInfo.Init(quest);
            }
        }
        if (currentShownedQuest != null)
        {
            ShowQuestInfo(currentShownedQuest);
        }
    }
    public void RemoveFollowQuest()
    {
        followQuestInfo.gameObject.SetActive(false);
    }
    public void SetQuestType(int type)
    {
        _currentPage = 0;
        currentQuestType = (QuestType)type;
        UpdateLabels(Player.Instance.questManager.quests);
    }
    public void SetQuestType(QuestType type)
    {
        SetQuestType((int)type);
    }
    public void ShowQuestInfo(Quest quest)
    {
        currentShownedQuest = quest;
        selectedQuestInfo.gameObject.SetActive(true);
        selectedQuestInfo.Init(quest);
    }
    public void ChangePage(int dir)
    {
        _currentPage += dir;
        int n = 0;
        foreach (QuestLabel label in _questLabels)
        {
            if (label.quest.questType == currentQuestType) n++;
        }
        if (_currentPage == -1)
        {
            _currentPage = (int)(n / labelsInPage);
        }else if(_currentPage > (int)(n / labelsInPage))
        {
            _currentPage = 0;
        }
        Player.Instance.questManager.UpdateMenu();
    }
}