using UnityEngine;

public class QuestGiverByStart : QuestGiver
{
    void Start()
    {
        Invoke("GiveQuest", 0.3f);
    }
}
