using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour // Одноразовый код
{
    [SerializeField] private List<Door> doors;
    private bool isClosed;
    [SerializeField] private WinPanel winPanel;
    private void Awake()
    {
        isClosed = false;
        OpenDoors();
    }
    public void OpenDoors()
    {
        foreach(var door in doors)
        {
            door.gameObject.SetActive(false);
        }
    }
    public void CloseDoors()
    {
        if (!isClosed)
        {
            foreach (var door in doors)
            {
                door.gameObject.SetActive(true);
            }
            isClosed = true;
       
        }
    }
    public void BossDead()
    {
        OpenDoors();
        winPanel.gameObject.SetActive(true);
        winPanel.Open();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            CloseDoors();
        }
    }
}
