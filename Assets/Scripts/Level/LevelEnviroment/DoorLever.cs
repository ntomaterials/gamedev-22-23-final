using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class DoorLever : MonoBehaviour
{
    [SerializeField] private Door door;

    [SerializeField] protected Sprite Closed;
    [SerializeField] protected Sprite Opened;
    [HideInInspector] public bool IsOpen { get; set; }
    protected bool CanOpen;
    protected InputHandler inputHandler;

    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip leverSound;
    protected virtual void Awake()
    {
        //IsOpen = false;
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onActionBtnUp += TryOpen;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
    public virtual void CheckOpen()
    {
        door.gameObject.SetActive(!IsOpen);///////////////// �������� �����
        if (IsOpen) spriteRenderer.sprite=Opened;
        else spriteRenderer.sprite=Closed;
        audioSource.PlayOneShot(leverSound);
    }
}
