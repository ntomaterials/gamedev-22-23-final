using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;

public class ShowCurses : MonoBehaviour
{
    [SerializeField] private Text cursesLabel;
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
                icon.SetTextToStacks();
            }
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
    public Text text;
    [HideInInspector]public int stacks;

    public void SetTextToStacks()
    {
        text.text = stacks.ToString();
    }
}