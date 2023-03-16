using UnityEngine;
using UnityEngine.UI;
[RequireComponent (typeof(Animator))]
public class Cutscene : MonoBehaviour
{
    private Animator animator;
    //[SerializeField] private AnimationClip cutsceneAnim;
    public event CutsceneEnded onCutsceneEnded;
    public delegate void CutsceneEnded();
    private OnPlayMenu mainCanvas;
    private Image image;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCanvas = FindObjectOfType<OnPlayMenu>();//�� ����, ���� �� � �������� ��������, ������� ���� ����� �������
        transform.parent = mainCanvas.transform;
        transform.localPosition = Vector3.zero;
        image=GetComponent<Image>();
        //image.raycastTarget=true;
    }
    public void PlayCutscene()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Play");
    }
    public void OnCutsceneEnd()
    {
        animator.Play("Idle");
        //image.raycastTarget=false;
        onCutsceneEnded?.Invoke();
    }
}
