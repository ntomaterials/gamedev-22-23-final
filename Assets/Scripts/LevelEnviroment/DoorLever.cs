using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class DoorLever : MonoBehaviour
{
    [SerializeField] private Door door;
    public bool IsOpen { get; set; }
    private bool CanOpen;
    private InputHandler inputHandler;

    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        //IsOpen = false;
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onActionBtnUp += TryOpen;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            CanOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            CanOpen = false;
        }
    }
    public void TryOpen()
    {
        if (CanOpen)
        {
            IsOpen = !IsOpen;
            CheckOpen();
        }
    }
    public void CheckOpen()
    {
        door.gameObject.SetActive(!IsOpen);///////////////// допилить можно
        if (IsOpen) spriteRenderer.color = Color.green;
        else spriteRenderer.color = Color.red;
    }
}
