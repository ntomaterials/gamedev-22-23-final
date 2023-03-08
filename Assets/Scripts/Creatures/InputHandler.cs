using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;
    [SerializeField] private Player player;
    [SerializeField] private Joystick joystick;

    public Vector2 inputAxis { get; private set; }
    private Vector2 _lastInputAxis;

    public event ActionBtnUp onActionBtnUp;
    public delegate void ActionBtnUp();
    
    public event MenuBtnUp onMenuBtnUp;
    public delegate void MenuBtnUp();

    public event DropBtnUp onDropBtnUp;
    public delegate void DropBtnUp();

    public event UseBtnUp onUseBtnUp;
    public delegate void UseBtnUp();

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
        if (joystick == null)
        {
            inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        else
        {
            inputAxis = new Vector2(joystick.Horizontal, joystick.Vertical);
            //if (joystick.Vertical > 0.8f) Jump();
        }
        
        
        if (!(inputAxis.x == 0 && _lastInputAxis.x != 0))
        {
            isXinput?.Invoke(true);
        }
        else isXinput?.Invoke(false);

        BaseCheck();
        if (!player.climbing)
        {
            DefaultMovementCheck();
        }
        else
        {
         ClimbingCheck();   
        }
        
        CheckForWeaponChange();
        
        _lastInputAxis = inputAxis;
    }

    private void ClimbingCheck()
    {
        player.Climb(new Vector2(inputAxis.x, inputAxis.y));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void Block()
    {
        player.Block();
    }
    public void Jump()
    {
        if (player.climbing)
        {
            player.StopClimbing();
            player.Jump(false);
        }
        else
        {
            player.Jump();
        }
        
    }
    public void Roll()
    {
        if (!player.climbing)
        {
        player.Roll();
        }
    }
    public void UseHeal(){
        onUseBtnUp?.Invoke();
    }
    public void Menu(){
        onMenuBtnUp?.Invoke();
    }
    public void Drop(){
        onDropBtnUp?.Invoke();
    }

    public void StopJump()
    {
        player.StopJump();
    }

    public void Crouch() => player.StartCrouch();
    
    public void StopCrouch() => player.StopCrouch();
    public void Attack() => player.StartBaseAttack();
    public void Interact()
    { 
        onActionBtnUp?.Invoke();
        if (!player.climbing && player.canClimb) player.StartClimbing();
        else player.StopClimbing();
    }

    public void NextWeapon()
    {
        player.NextWeapon();
    }

    private void BaseCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) onMenuBtnUp?.Invoke();
        if (Input.GetKeyUp(KeyCode.E))
        {
            onActionBtnUp?.Invoke();
            if (!player.climbing && player.canClimb) player.StartClimbing();
            else player.StopClimbing();
        }
        if (Input.GetKeyDown(KeyCode.Q)) onDropBtnUp?.Invoke();
        if (Input.GetKeyDown(KeyCode.F)) onUseBtnUp?.Invoke();
    }

    private void DefaultMovementCheck()
    {
        player.Run(inputAxis.x);
        if (joystick != null) return;
        if (Input.GetButtonDown("Fire1")) player.StartBaseAttack();
        
        else if(Input.GetMouseButtonDown(1)) player.Block();

        if (Input.GetKeyUp(KeyCode.S)) player.Roll();

        if (Input.GetKey(KeyCode.LeftShift)) player.StartCrouch();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }else if (Input.GetKeyUp(KeyCode.Space))
        {
            player.StopJump();
        }
        else player.StopCrouch();
    }
    

    private void CheckForWeaponChange() // судя по всему без новой InputSystem по другому никак
    {
        if (!Input.anyKeyDown) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player.IsWeaponsActive[0]) player.SetWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.IsWeaponsActive[1]) player.SetWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (player.IsWeaponsActive[2]) player.SetWeapon(2);
        }
    }
}
