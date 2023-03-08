using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [field: SerializeField] public List<Level> allLevels { get; private set; }
    //[SerializeField] private AudioSource musicObj;
    [SerializeField] private Cutscene startCutscene;
    //[HideInInspector] public int loadingLevelID; //�������, ������� �� ����� ���������
    public Level levelOnScene { get; private set; }//�������, ������� ������ �������� �� �����
    public int lastSavedLevelID;
    private SaveLoadManager saveLoadManager;
    [SerializeField] private Player player;
    private void Awake()
    {
        //player = FindObjectOfType<Player>();
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
    public void LoadLevel(int loadingID, int fireID=0)
    {
        if (levelOnScene != null)
        {
            if (levelOnScene != allLevels[loadingID])
            {
                Destroy(levelOnScene.gameObject);
                Loading(loadingID, fireID);
            }
        }
        else Loading(loadingID, fireID);
    }
    public void StartLoadNewGame()
    {
        startCutscene.PlayCutscene();
    }
    public void LoadNewGame()
    {
        LoadLevel(0, 0);
        levelOnScene.lastFireID = 0;
        levelOnScene.LoadLevelObjects(null);
    }
    private void Loading(int loadingID, int fireID=0)
    {
        levelOnScene= Instantiate(allLevels[loadingID].gameObject, Vector2.zero, Quaternion.identity).GetComponent<Level>();
        levelOnScene.id = loadingID;
        levelOnScene.lastFireID=fireID;
        Debug.Log(levelOnScene.lastFireID);
        //AudioSource music = Instantiate(musicObj).GetComponent<AudioSource>();
        //music.clip = levelOnScene.levelMusic;
        //player=Player.Instance;
        if(player==null) player=FindObjectOfType<Player>();
        if(levelOnScene.lastFireID==0)
        {
        player.transform.position = levelOnScene.playerSpawn.position;
        Debug.Log("Spawn on Start");
        }
        else
        {
            player.transform.position=levelOnScene.saveFires[levelOnScene.lastFireID].playerSpawn.position;
            Debug.Log("Spawn on Fire");
            //Debug.Log(levelOnScene.saveFires[levelOnScene.lastFireID].playerSpawn.position);
            //Debug.Log(player.transform.position);
        }
    }
}
