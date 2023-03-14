using UnityEngine;

public class QuestDeathMark : MonoBehaviour
{
    public string questCodeName = "q_1";
    [Range(0, 1)] public float comletePercent=1f;
    private void OnDestroy()
    {
        
    }
}
