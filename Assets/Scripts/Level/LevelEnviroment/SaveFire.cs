using UnityEngine;
[RequireComponent (typeof(AudioSource))]
public class SaveFire : MonoBehaviour
{
    [HideInInspector] public int id;
    public Transform playerSpawn;

    public event SavedOnFire savedOnFire;
    public delegate void SavedOnFire(int fireID);

    //[SerializeField] private EnemyHolder enemyHolder;
    private bool canUse;
    private InputHandler inputHandler;
    private Player player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip saveSound;
    //private SaveLoadManager saveLoadManager;
    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onActionBtnUp += Save;
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
        //saveLoadManager = FindObjectOfType<SaveLoadManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canUse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            canUse = false;
        }
    }
    private void Save()
    {
        if (canUse)
        {
            if (saveSound != null) audioSource.PlayOneShot(saveSound);
            savedOnFire?.Invoke(id);
            player.FullHeal();
            player.GetKvas(player.KvasInventory.maxCapacity);
        }
    }
}
