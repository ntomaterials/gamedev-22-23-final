using UnityEngine;


public enum QuestType { Plot=0, Parkour=1, Battle=2, Completed=3}
[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public string rewardInfo;
    public QuestType questType;
    public int expirienceReward;
    public float progress;
    public bool isCompleted = false;
    public bool follow = false;
    public string codeName="q_1";
}
