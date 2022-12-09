using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    private bool isOpened;
    private InputHandler inputHandler;
    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onActionBtnUp += Close;
        isOpened = false;
        gameObject.SetActive(false);
    }
    public void Close()
    {
        if (isOpened)
        {
            gameObject.SetActive(false);
            isOpened = false;
        }
    }
    public void Open()
    {
        gameObject.SetActive(true);
        isOpened = true;
    }
}
