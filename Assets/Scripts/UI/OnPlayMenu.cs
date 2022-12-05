using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlayMenu : MainMenu
{
    public bool IsPaused { get; protected set; }
    private InputHandler inputHandler;
    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onMenuBtnUp += ListenEscape;
        base.Awake();
        ClosePanel();
    }
    public override void OpenPanel(MenuPanel panel)
    {
        panel.gameObject.SetActive(true);
        if (activeMenuPanel != panel) activeMenuPanel.gameObject.SetActive(false);
        activeMenuPanel = panel;
    }
    protected override void OpenMainPanel()
    {
        base.OpenMainPanel();
        IsPaused = true;
        CheckPause();
    }
    public override void ClosePanel()
    {
        if(activeMenuPanel!=mainPanel) base.ClosePanel();
        else
        {
            IsPaused = false;
            CheckPause();
            activeMenuPanel.gameObject.SetActive(false);
        }
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(GlobalConstants.MainMenuSceneIndex);
    }
    private void CheckPause()
    {
        if (IsPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
    public override void ListenEscape()
    {
        if (!IsPaused) OpenMainPanel();
        else ClosePanel();
    }
}
