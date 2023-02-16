using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class DoorLever : MonoBehaviour
{
    [SerializeField] private Door door;

    [SerializeField] private Sprite Closed;
    [SerializeField] private Sprite Opened;
    [HideInInspector] public bool IsOpen { get; set; }
    private bool CanOpen;
    private InputHandler inputHandler;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField] private AudioClip leverSound;
    private void Awake()
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
    public void CheckOpen()
    {
        door.gameObject.SetActive(!IsOpen);///////////////// допилить можно
        if (IsOpen) spriteRenderer.sprite=Opened;
        else spriteRenderer.sprite=Closed;
        audioSource.PlayOneShot(leverSound);
    }
}
