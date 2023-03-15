using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(BoxCollider2D))]
public class DialogTrigger : MonoBehaviour
{
    //[SerializeField] protected DialogWindow dialogWindow;
    [field:SerializeField] public Dialog dialog{get; private set;}
    private DialogManager dialogManager;
    private Animator _animator;
    private BoxCollider2D _collider;
    private bool canShow;
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
        private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            HidePanel();
            canShow = false;
        }   
    }
    private  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canShow = true;
        }
    }

    private void TryShowWindow()
    {
        if (!canShow) return;
        if (dialogWindow.gameObject.activeInHierarchy) HidePanel();
        else 
        {
            ShowWindow();
        }
    }

    private void ShowWindow()
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

    private void OnDestroy()
    {
        InputHandler.Instance.onActionBtnUp -= TryShowWindow;
    }
}
