using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour // ����������� ���
{
    [SerializeField] private List<Door> doors;
    private bool isClosed;
   // [SerializeField] private WinPanel winPanel;
    [SerializeField] private DialogWithInteractable _kudesnic;
    private Collider2D collider;
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = true;
        isClosed = false;
        OpenDoors();
        _kudesnic.gameObject.SetActive(false);
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
        //winPanel.gameObject.SetActive(true);
        //winPanel.Open();
        _kudesnic.gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            CloseDoors();
            collider.enabled=false;
        }
    }
}
