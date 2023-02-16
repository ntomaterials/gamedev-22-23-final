using UnityEngine;

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
        inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
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
            player.StopClimbing();
            player.Jump(false);
        }
    }

    private void BaseCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) onMenuBtnUp?.Invoke();
        if (Input.GetKeyUp(KeyCode.E))
        {
            onActionBtnUp?.Invoke();
            if (!player.climbing && player.canClimb)
            {
                player.StartClimbing();
            }
            else
            {
                player.StopClimbing();
            }
        }
    }

    private void DefaultMovementCheck()
    {
        player.Run(inputAxis.x);
        if (Input.GetButtonDown("Fire1")) player.StartBaseAttack();
        
        else if(Input.GetMouseButtonDown(1)) player.Block();

        if (Input.GetKeyUp(KeyCode.S)) player.Roll();

        if (Input.GetKey(KeyCode.LeftShift)) player.StartCrouch();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
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
            player.SetWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) player.SetWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) player.SetWeapon(2);
    }
}
