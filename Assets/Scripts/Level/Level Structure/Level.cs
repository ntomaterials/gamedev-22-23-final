using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [HideInInspector] public int id { get; set; }
    public int lastFireID;
    [field: SerializeField] public List<DoorLever> doorLevers { get; private set; }
    [field: SerializeField] public List<SaveFire> saveFires { get; private set; }

    public EnemyHolder enemyHolder;
    [field: SerializeField] public Transform playerSpawn { get; private set; }
    //[field: SerializeField] public AudioClip levelMusic { get; private set; }

    private LevelsData levelsData;
    private SaveLoadManager saveLoadManager;
    private Player player;
    private void Awake()
    {
        for (int i = 0; i < saveFires.Count; i++)
        {
            saveFires[i].id = i;
            saveFires[i].savedOnFire += SaveLevelData;
        }
        player = FindObjectOfType<Player>();
        levelsData = FindObjectOfType<LevelsData>();
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
    }
    public void SaveLevelData(int fireID)
    {
        lastFireID = fireID;
        levelsData.lastSavedLevelID = id;

        enemyHolder.ReloadEnemies();
        saveLoadManager.SaveData(lastFireID);
    }
    public void LoadLevelObjects(List<bool> leversStates)
    {
        if (levelsData.lastSavedLevelID == id && !(id == 0 && lastFireID == 0))
        {
            Vector3 pos = saveFires[lastFireID].playerSpawn.position;
            //player.transform.position =new Vector3(pos.x, pos.y, 0);
        }
        //else player.transform.position = playerSpawn.position;
        enemyHolder.ReloadEnemies();

        /*if (saveLoadManager.deathMarker != null)
        {
            DeathMarker marker = Instantiate(saveLoadManager.deathMarker, saveLoadManager.deathMarkerPos.position, Quaternion.identity);
            marker.Score = saveLoadManager.deathMarker.Score;
            //DontDestroyOnLoad(marker);
        }*/

        if (leversStates == null) // ���� ����� ����, ��� ����� �������
        { 
            leversStates = new List<bool>();
            for (int i=0; i<doorLevers.Count; i++)
            {
                leversStates.Add(false);
            }
        }
        for(int i=0; i<doorLevers.Count; i++)
        {
            DoorLever lever = doorLevers[i];
            lever.IsOpen = leversStates[i];
            lever.CheckOpen();
        }
    }
}
