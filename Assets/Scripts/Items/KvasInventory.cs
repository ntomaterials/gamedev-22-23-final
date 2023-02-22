using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KvasInventory : PlayerInventory
{
    protected override void Start()
    {
        UpdateCanvas();
        //_dropPoint = Player.Instance.dropPoint;
        InputHandler.Instance.onUseBtnUp += UseKvas;
    }
    public void UseKvas()
    {
        if (Player.Instance.health < Player.Instance.maxHealth)
        {
            if (products.Count > 0)
            {
                baseProduct.OnUse();
                products.RemoveAt(products.Count - 1);
                UpdateCanvas();
            }
        }
    }
}
