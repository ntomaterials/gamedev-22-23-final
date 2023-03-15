using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(BoxCollider2D))]
public class DialogTrigger : MonoBehaviour
{
    //[SerializeField] protected DialogWindow dialogWindow;
    public Dialog dialog;
    private DialogManager dialogManager;
    protected Animator _animator;
    protected BoxCollider2D _collider;
    protected bool canShow;
    private DialogWindow dialogWindow;
    private void Start() 
    {
        dialogManager=DialogManager.Instance;
        dialogWindow=DialogWindow.Instance;

        InputHandler.Instance.onActionBtnUp += TryShowWindow;
        dialogManager.onDialogEnd+=HidePanel;

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

    protected void TryShowWindow()
    {
        if (!canShow) return;
        if (dialogWindow.gameObject.activeInHierarchy) HidePanel();
        else 
        {
            ShowWindow();
        }
    }

    protected virtual void ShowWindow()
    {
        dialogWindow.gameObject.SetActive(true);
        TriggerDialog();
    }
        public void TriggerDialog()
    {
        dialogManager.StartDialog(dialog);
    }

    public void HidePanel()
    {
        dialogWindow.gameObject.SetActive(false);
    }

    protected void OnDestroy()
    {
        InputHandler.Instance.onActionBtnUp -= TryShowWindow;
    }
}
