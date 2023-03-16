using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class DialogAnswer
{
    [field: SerializeField] public bool IsEndReplic {get; private set;}
    public Dialog playerAnswer;
    public Dialog nextDialog;
    public string answerText;
    public QuestGiver questGiver;
}
