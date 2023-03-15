using UnityEngine;
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(BoxCollider2D))]
public class DialogWithInteractable : MonoBehaviour
{
    [SerializeField] protected GameObject dialogScreen;

    protected Animator _animator;
    protected BoxCollider2D _collider;
    protected bool canShow;
    protected virtual void Start()
    {
        InputHandler.Instance.onActionBtnUp += TryShowTutorial;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        canShow = false;
        HidePanel();
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            HidePanel();
            canShow = false;
        }   
    }
    protected virtual  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canShow = true;
        }
    }

    protected void TryShowTutorial()
    {
        if (!canShow) return;
        if (dialogScreen.activeInHierarchy) HidePanel();
        else 
        {

            ShowTutorial();
        }
    }

    protected virtual void ShowTutorial()
    {
        dialogScreen.gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        dialogScreen.gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        InputHandler.Instance.onActionBtnUp -= TryShowTutorial;
    }
}
