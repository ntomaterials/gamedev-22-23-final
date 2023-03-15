using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    public event EndingDialog onDialogEnd;
    public delegate void EndingDialog();
    private DialogWindow dialogWindow;
    private Player player;
    private Dialog nowDialog;
    private Dialog nextDialog;
    private Queue<string> sentences= new Queue<string>();
    //private Animator dialogWindow;
    private void Awake() 
    {
        Instance=this;
        nowDialog=null;
        nextDialog=null;
    }
    private void Start() {
        dialogWindow=DialogWindow.Instance;   
        player=Player.Instance;  
    }
    public void StartDialog(Dialog dialog, Dialog next=null)
    {
        nowDialog=dialog;
        nextDialog=next;
        player.StopMove();
        dialogWindow.HideAnswerWindow();
        dialogWindow.nameText.text=nowDialog.npcName;
        sentences.Clear();
        foreach (string sentence in nowDialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count==0) 
        {
            if(nextDialog==null)
            {
            dialogWindow.ShowAnswerWindow();
            //EndDialog();
            //return;
            }
            else 
            {
                StartDialog(nextDialog);
                return;
            }
        }
        string sentence=sentences.Dequeue();
        dialogWindow.textArea.text=sentence;
    }
    public void EndDialog()
    {
        player.ContinueMove();
        dialogWindow.HideAnswerWindow();
        onDialogEnd?.Invoke();
        //dialogWindow.SetBool("IsOpened", false);
    }
}
