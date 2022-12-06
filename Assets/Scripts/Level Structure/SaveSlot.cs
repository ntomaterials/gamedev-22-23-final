using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveSlot : MonoBehaviour
{
    [field: SerializeField] public string fileName { get; private set; }
    public string name;
    public float playedTime;

    private SaveLoadManager saveLoadManager;
    private void Awake()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
    }
    public void LoadGame()
    {
        saveLoadManager.fileName = fileName;
        SceneManager.LoadScene(GlobalConstants.GameSceneIndex);
    }
}
