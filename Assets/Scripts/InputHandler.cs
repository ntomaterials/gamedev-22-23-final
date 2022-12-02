﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    private Vector2 _lastInputAxis;

    public event ActionBtnUp onActionBtnUp;
    public delegate void ActionBtnUp();  

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
        if (Input.GetKeyUp(KeyCode.E))
        {
            onActionBtnUp.Invoke();
        }
        
        _lastInputAxis = inputAxis;
    }
}
