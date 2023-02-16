using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [field: SerializeField] public List<Level> allLevels { get; private set; }
    [SerializeField] private GameObject musicObj;
    [SerializeField] private Cutscene startCutscene;
    //[HideInInspector] public int loadingLevelID; //Уровень, который мы хотим загрузить
    public Level levelOnScene { get; private set; }//Уровень, который сейчас загружен на карте
    public int lastSavedLevelID;
    private SaveLoadManager saveLoadManager;
    private Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        for (int i=0; i<allLevels.Count; i++)
        {
            allLevels[i].id = i;
        }

        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        saveLoadManager.levelsData = this;
        Debug.Log(saveLoadManager.levelsData);
        saveLoadManager.LoadData();
        startCutscene.onCutsceneEnded += LoadNewGame;
    }
    public void LoadLevel(int loadingID)
    {
        if (levelOnScene != null)
        {
            if (levelOnScene != allLevels[loadingID])
            {
                Destroy(levelOnScene.gameObject);
                Loading(loadingID);
            }
        }
        else Loading(loadingID);
    }
    public void StartLoadNewGame()
    {
        startCutscene.PlayCutscene();
    }
    public void LoadNewGame()
    {
        LoadLevel(0);
        levelOnScene.lastFireID = 0;
        levelOnScene.LoadLevelObjects(null);
    }
    private void Loading(int loadingID)
    {
        levelOnScene= Instantiate(allLevels[loadingID].gameObject, Vector2.zero, Quaternion.identity).GetComponent<Level>();
        levelOnScene.id = loadingID;
        Instantiate(musicObj);
        //player.transform.position = levelOnScene.playerSpawn.position;
    }
}
