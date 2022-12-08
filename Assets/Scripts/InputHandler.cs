﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    [SerializeField] private Player player;

    public Vector2 inputAxis { get; private set; }
    private Vector2 _lastInputAxis;

    public event ActionBtnUp onActionBtnUp;
    public delegate void ActionBtnUp();
    
    public event MenuBtnUp onMenuBtnUp;
    public delegate void MenuBtnUp();

    public event Xinput isXinput;
    public delegate void Xinput(bool isInput);
    //private MainMenu menu;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (player == null) return;
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            player.Roll();
        } // нужно чтобы при смене направления движения не срабатывала idle анимация
        else if (!(inputAxis.x == 0 && _lastInputAxis.x != 0))
        {
            player.Run(inputAxis.x);
            isXinput?.Invoke(true);
        }
        else isXinput?.Invoke(false);

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
            onActionBtnUp?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onMenuBtnUp?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            player.Roll();
        }
        CheckForWeaponChange();
        _lastInputAxis = inputAxis;
    }

    private void CheckForWeaponChange() // судя по всему без новой InputSystem по другому никак
    {
        if (!Input.anyKeyDown) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.SetWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) player.SetWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) player.SetWeapon(2);
    }
}
