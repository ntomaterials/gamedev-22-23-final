﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    [SerializeField] private Player player;

    private Vector2 _lastInputAxis;

    public event ActionBtnUp onActionBtnUp;
    public delegate void ActionBtnUp();
    
    public event MenuBtnUp onMenuBtnUp;
    public delegate void MenuBtnUp();
    //private MainMenu menu;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (player == null) return;
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        // нужно чтобы при смене направления движения не срабатывала idle анимация
        if (!(inputAxis.x == 0 && _lastInputAxis.x != 0))
        {
            player.Run(inputAxis.x);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }else if (Input.GetKeyUp(KeyCode.Space))
        {
            player.StopJump();
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            player.StartBaseAttack();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            player.Block();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            onActionBtnUp.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            player.Roll();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onMenuBtnUp.Invoke();
        }
        _lastInputAxis = inputAxis;
    }
}
