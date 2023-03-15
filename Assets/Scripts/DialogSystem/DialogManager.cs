using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    public event EndingDialog onDialogEnd;
    public delegate void EndingDialog();
    private DialogWindow dialogWindow;
    private Player player;
    private AnswerWindow answerWindow;
    private Dialog nowDialog;
    private Dialog nextDialog;
    private Queue<string> sentences= new Queue<string>();
    private Queue<AudioClip> voices=new Queue<AudioClip>();
    [SerializeField] private AudioSource _audioSource;
    //private Animator dialogWindow;
    private void Awake() 
    {
        Instance=this;
        nowDialog=null;
        nextDialog=null;
        //_audioSource=GetComponent<AudioSource>();
    }
    private void Start() {
        dialogWindow=DialogWindow.Instance;   
        player=Player.Instance;  
        answerWindow=AnswerWindow.Instance;
    }
    public void StartDialog(Dialog dialog, Dialog next=null)
    {
        nowDialog=dialog;
        nextDialog=next;
        player.StopMove();
        dialogWindow.HideAnswerWindow();
        dialogWindow.nameText.text=nowDialog.npcName;
        sentences.Clear();
        voices.Clear();
        foreach(string sentence in nowDialog.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach(AudioClip voice in nowDialog.voices)
        {
            voices.Enqueue(voice);
        }
        /*for(int i=0; i<sentences.Count;i++)
        {
            sentences.Enqueue(nowDialog.sentences[i]);
            //if(nowDialog.voices.Length==nowDialog.sentences.Length) voices.Enqueue(nowDialog.voices[i]);
            //voices.Enqueue(nowDialog.voices[i]);
        }*/
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        _audioSource.Stop();
        if(sentences.Count==0) 
        {
            if(nextDialog==null)
            {
            dialogWindow.ShowAnswerWindow();
            if(nowDialog.answers.Count!=0) answerWindow.FillAnswers(nowDialog.answers);
            }
            else 
            {
                StartDialog(nextDialog);
                return;
            }
        }
        string sentence=sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(PrintText(sentence));

        AudioClip voice=null;
        if(voices!=null) voice =voices.Dequeue();
        if(voice!=null) _audioSource.PlayOneShot(voice);

    }
    private IEnumerator PrintText(string sentence)
    {

        
        dialogWindow.textArea.text="";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogWindow.textArea.text+=letter;
            yield return new WaitForSeconds(GlobalConstants.defaultDialogDelay);
        }
    }
    public void EndDialog()
    {
        player.ContinueMove();
        dialogWindow.HideAnswerWindow();
        answerWindow.ClearAnswers();
        onDialogEnd?.Invoke();
        //dialogWindow.SetBool("IsOpened", false);
    }
}
