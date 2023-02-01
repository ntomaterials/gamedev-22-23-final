using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public MenuPanel activeMenuPanel { get; protected set; }
    [field: SerializeField]public MenuPanel mainPanel { get; protected set; }
    protected void Awake()
    {
        activeMenuPanel = mainPanel;
        activeMenuPanel.gameObject.SetActive(true);
    }
    public virtual void OpenPanel(MenuPanel panel)
    {
        panel.gameObject.SetActive(true);
        activeMenuPanel.gameObject.SetActive(false);
        activeMenuPanel = panel;
    }
    protected virtual void OpenMainPanel()
    {
        OpenPanel(mainPanel);
    }
    public virtual void ClosePanel()
    {
        if (activeMenuPanel.parentPanel == null) OpenMainPanel();
        else OpenPanel(activeMenuPanel.parentPanel);
    }
    /*public virtual void LoadGame()
    {
        SceneManager.LoadScene(GlobalConstants.GameSceneIndex);
    }*/
    public virtual void ListenEscape()
    {
        if (activeMenuPanel != mainPanel) ClosePanel();
        else return;
    }
    public virtual void QuitGame()
    {
        Application.Quit();
    }
}
