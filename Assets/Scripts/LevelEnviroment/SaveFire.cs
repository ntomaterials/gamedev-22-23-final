using UnityEngine;

public class SaveFire : MonoBehaviour
{
    [HideInInspector] public int id;
    public Transform playerSpawn;

    [SerializeField] private EnemyHolder enemyHolder;
    private bool canUse;
    private InputHandler inputHandler;
    private Player player;
    private SaveLoadManager saveLoadManager;
    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.onActionBtnUp += Save;
        player = FindObjectOfType<Player>();
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
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
            player.FullHeal();
            enemyHolder.ReloadEnemies();
            saveLoadManager.SaveData(id);
        }
    }
}
