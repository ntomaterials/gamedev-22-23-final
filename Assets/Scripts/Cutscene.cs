using UnityEngine;
[RequireComponent (typeof(Animator))]
public class Cutscene : MonoBehaviour
{
    private Animator animator;
    //[SerializeField] private AnimationClip cutsceneAnim;
    public event CutsceneEnded onCutsceneEnded;
    public delegate void CutsceneEnded();
    private OnPlayMenu mainCanvas;
    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCanvas = FindObjectOfType<OnPlayMenu>();//не знаю, есть ли у канвасов иерархия, поэтому пока такой костыль
        transform.parent = mainCanvas.transform;
        transform.localPosition = Vector3.zero;
    }
    public void PlayCutscene()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Play");
    }
    public void OnCutsceneEnd()
    {
        animator.Play("Idle");
        onCutsceneEnded?.Invoke();
    }
}
