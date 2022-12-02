using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurses : MonoBehaviour
{
    [SerializeField] private CurseIcon[] curseIcons;

    private struct CurseInfo
    {
        public CurseType type;
        public int stacks;
    }

    private void Update()
    {
        ResetCurseIconsStacks();
        List <Curse> curses = Player.Instance.GetCurses();
        //curses = curses.OrderBy(w => w.GetType().Name).ToList();

        foreach (var curse in curses)
        {
            GetCurseIconByType(curse.type).stacks += curse.stacks;
        }

        foreach (var icon in curseIcons)
        {
            if (icon.stacks == 0 || icon.stacks >= GlobalConstants.S * 2)
            {
                icon.gameObject.SetActive(false);
            }
            else
            {
                icon.gameObject.SetActive(true);
            }
            icon.Update();
        }
    }

    private void ResetCurseIconsStacks()
    {
        foreach (var c in curseIcons)
        {
            c.stacks = 0;
        }
    }
    
    private CurseIcon GetCurseIconByType(CurseType type)
    {
        foreach (var c in curseIcons)
        {
            if (c.type == type) return c;
        }
        throw new Exception("Can not find object");
    }
}


[System.Serializable]
public class CurseIcon
{
    public CurseType type;
    [Tooltip("Ссылка на обьект в канвасе")] public GameObject gameObject;
    public GameObject fullscreenEffect;
    public Text text;
    [HideInInspector]public int stacks;

    public void Update()
    {
        if (stacks == 1) text.text = ""; 
        else text.text = stacks.ToString();
        if (stacks > GlobalConstants.S)
        {
            fullscreenEffect.SetActive(true);
        }
        else
        {
            fullscreenEffect.SetActive(false);
        }
    }
}