using UnityEngine;
using System.Collections.Generic;

public class ActivateWhenQuestFollowing : MonoBehaviour
{
    [SerializeField] private string questCodeName="q_1";
    [SerializeField] private List<GameObject> gameObjects;
    private void Start()
    {
        InvokeRepeating("UpdateGameObjects", 0f, 0.2f);
    }
    private bool Check()
    {
        print(Player.Instance.questManager.followedQuest);
        if (Player.Instance.questManager.followedQuest != null)
        {
            if (Player.Instance.questManager.followedQuest.codeName == questCodeName)
            {
                return true;
            }
        }
        return false;
    }
    private void UpdateGameObjects()
    {
        bool act = Check();
        foreach(GameObject i in gameObjects)
        {
            i.SetActive(act);
        }
    }
}
