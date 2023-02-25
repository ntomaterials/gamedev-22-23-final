using UnityEngine;
public class InteractableFinish : Finish
{
    protected bool canUse;
    protected override void Awake()
    {
        base.Awake();
        canUse = false;
        InputHandler.Instance.onActionBtnUp += LoadLevel;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) canUse = true;
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) canUse = false;
    }
    public void LoadLevel()
    {
        if (canUse)
        {
            LoadNextLevel();
        }
    }
}
