using UnityEngine;


public enum QuestType { Plot=0, Other=1, Completed=2}
[System.Serializable]
public class Quest
{
    public string title;
    [TextArea] public string description;
    public QuestType questType;
    public int expirienceReward;
    public float progress;
    public bool isCompleted = false;
    public bool follow = false;
    public string codeName="q_1";
}
