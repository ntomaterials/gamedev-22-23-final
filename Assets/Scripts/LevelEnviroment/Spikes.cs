using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(GlobalConstants.PlayerLayer == collision.gameObject.layer)
        {    
            Player player = collision.gameObject.GetComponent<Player>();
            player.StartCoroutine(player.Die());
        }
    }
}
