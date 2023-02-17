using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meet : Product
{
    private bool canTake;
    private PlayerInventory meetInventory;
    protected override void Start()
    {
        base.Start();
        canTake = false;
        meetInventory = Player.Inventory;
        InputHandler.Instance.onActionBtnUp += TakeMeet;
    }
    protected override void DoOnEnter()
    {
        base.DoOnEnter();
        canTake = true;
    }
    protected override void DoOnExit()
    {
        base.DoOnExit();
        canTake = false;
    }
    public void TakeMeet()
    {
        if (canTake)
        {
            if (meetInventory.CanAdd())
            {
                meetInventory.AddProduct(new Meet());
                Destroy(gameObject);
            }
        }
    }
}
