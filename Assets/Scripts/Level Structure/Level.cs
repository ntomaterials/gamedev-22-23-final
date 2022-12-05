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
        player.transform.position = saveFires[lastFireID].playerSpawn.position;
        enemyHolder.ReloadEnemies();

        if (leversStates == null) // Если новая игра, все двери закрыты
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
