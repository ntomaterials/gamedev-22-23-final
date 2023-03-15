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
    private Queue<string> sentences= new Queue<string>();
    //private Animator dialogWindow;
    private void Awake() 
    {
        Instance=this;

    }
    private void Start() {
        dialogWindow=DialogWindow.Instance;   
        player=Player.Instance;  
    }
    public void StartDialog(Dialog dialog)
    {

        player.StopMove();
        dialogWindow.nameText.text=dialog.npcName;
        sentences.Clear();
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count==0) 
        {
            EndDialog();
            return;
        }
        string sentence=sentences.Dequeue();
        dialogWindow.textArea.text=sentence;
    }
    public void EndDialog()
    {
        player.ContinueMove();
        onDialogEnd?.Invoke();
        //dialogWindow.SetBool("IsOpened", false);
    }
}
