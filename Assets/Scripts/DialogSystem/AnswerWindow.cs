using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerWindow : MonoBehaviour
{
    public static AnswerWindow Instance;
    [SerializeField] private AnswerBtn BtnPrefab;
    [SerializeField] private AnswerBtn endReplicBtn;
    [SerializeField] private List<AnswerBtn> answersInWindow=new List<AnswerBtn>();
private void Awake() 
{
    Instance=this;
}
public void FillAnswers(List<DialogAnswer> answers)
{
    ClearAnswers();
    if(answers!=null)
    {
    for(int i=0; i<answers.Count; i++)
    {
        CreateBtn(BtnPrefab, answers[i]);
    }
    }
    CreateBtn(endReplicBtn, endReplicBtn.answer);
}
private void CreateBtn(AnswerBtn btn, DialogAnswer answer)
{
        AnswerBtn answerBtn=Instantiate(btn, transform.position, Quaternion.identity);
        answerBtn.answer=answer;
        answerBtn.transform.SetParent(transform);
        answerBtn.transform.localScale=new Vector3(1,1,1);
        answersInWindow.Add(answerBtn);
}
public void ClearAnswers()
{
    foreach(Transform child in transform)
    {
        Destroy(child.gameObject);
    }
    answersInWindow=new List<AnswerBtn>();
}
}
