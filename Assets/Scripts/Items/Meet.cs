using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meet : Product
{
    //[SerializeField] private GameObject obj;
    protected override void Start()
    {
        myInventory = Player.Instance.MeetInventory;
        base.Start();
    }
}
