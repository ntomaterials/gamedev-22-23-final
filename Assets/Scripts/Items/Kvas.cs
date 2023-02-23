using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kvas : Product
{
    [SerializeField] private int heal;
    protected override void Start()
    {
        myInventory = Player.Instance.KvasInventory;
        base.Start();
    }
    public override void OnUse()
    {
            Player.Instance.Heal(heal);
            base.OnUse();
    }
}
