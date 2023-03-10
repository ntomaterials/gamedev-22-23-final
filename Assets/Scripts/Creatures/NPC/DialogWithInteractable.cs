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
        HideTutorial();
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            HideTutorial();
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
        //float dist = (Player.Instance.transform.position - transform.position).magnitude;
        //if (dist > _collider.size.x/2) return;
        print("beba");
        if (dialogScreen.activeInHierarchy) HideTutorial();
        else 
        {

            ShowTutorial();
        }
    }

    protected virtual void ShowTutorial()
    {
        dialogScreen.gameObject.SetActive(true);
    }

    public void HideTutorial()
    {
        dialogScreen.gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        InputHandler.Instance.onActionBtnUp -= TryShowTutorial;
    }
}
