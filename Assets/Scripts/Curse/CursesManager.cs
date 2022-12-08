using System;
using System.Collections.Generic;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Vignette = UnityEngine.Rendering.Universal.Vignette;

public class CursesManager : MonoBehaviour
{
    public static CursesManager Instance;
    public int S=25;
    [SerializeField] private CurseIcon[] curseIcons;
    public bool disableFullscreenEffect;

    [SerializeField] private CinemachineVolumeSettings postProcessVolume;
    [HideInInspector]public Vignette vignette;
    
    private void Awake()
    {
        Instance = this;
        postProcessVolume.m_Profile.TryGet(out vignette);
        vignette.intensity.value = 0f;
    }

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
            if (icon.stacks == 0 || icon.stacks >= S * 2)
            {
                icon.image.gameObject.SetActive(false);
            }
            else
            {
                icon.image.gameObject.SetActive(true);
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
    [Tooltip("Ссылка на обьект в канвасе")] public Image image;
    public GameObject fullscreenEffect;
    public Text text;
    [HideInInspector] public int stacks;

    public void Update()
    {
        if (stacks == 1) text.text = ""; 
        else text.text = stacks.ToString();
        float alpha = (float)stacks / (float)(CursesManager.Instance.S * 2) * 0.9f + 0.1f;
        Color c = image.color;
        c.a = alpha;
        image.color = c;
        
        if (stacks >= CursesManager.Instance.S)
        {
            fullscreenEffect.SetActive(true);
        }
        else
        {
            fullscreenEffect.SetActive(false);
        }

        if (CursesManager.Instance.disableFullscreenEffect)
        {
            if (stacks >= CursesManager.Instance.S * 2)
            {
                fullscreenEffect.SetActive(false);
            }
        }
    }
}