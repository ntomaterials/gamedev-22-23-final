using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string filePath;
    private Save save = new Save();

    public string fileName="reservSave";

    public LevelsData levelsData;
    //public DeathMarker deathMarker;
   // public Transform deathMarkerPos;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SetPath();
    }
    private void SetPath()
    {
        filePath = Application.persistentDataPath + "/" + fileName+ ".save";
        Debug.Log(filePath);
    }
    public void SaveData(int fireID)
    {
        SetPath();
        //��-�� ��� ��������������, ������� ������
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);
        Save save = new Save();
        //��������� � ��������� ������
        save.SavedLevelID = levelsData.lastSavedLevelID;
        save.SavedFireID = fireID;
        save.SaveLeversState(levelsData.levelOnScene.doorLevers);/////
        save.quests = Player.Instance.questManager.quests;

        bf.Serialize(fs, save);
        fs.Close();
        Debug.Log("Saved");
    }
    //=====================================
    public void LoadData()
    {
        SetPath();
        //���� ���� ����������, ������� ������
        if (!File.Exists(filePath))
        {
            Debug.Log("No files?");
            NewGame();
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);
        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

        //======================
        levelsData.LoadLevel(save.SavedLevelID, save.SavedFireID);
        //levelsData.levelOnScene.lastFireID = save.SavedFireID;
        levelsData.levelOnScene.LoadLevelObjects(save.leversStates);
        //if(levelsData.levelOnScene.lastFireID!=0) Player.Instance.transform.position=levelsData.levelOnScene.saveFires[levelsData.levelOnScene.lastFireID].playerSpawn.position;
        Player.Instance.questManager.quests = save.quests;
    }
    public void NewGame()
    {
        Debug.Log("New Game");
        levelsData.StartLoadNewGame();
    }

    [System.Serializable]
    public class Save
    {
        public int SavedLevelID;
        public int SavedFireID;
        public List<bool> leversStates=new List<bool>();
        public List<Quest> quests = new List<Quest>();
        public void SaveLeversState(List<DoorLever> levers)
        {
            foreach (var lever in levers)
            {
                leversStates.Add(lever.IsOpen);
            }
        }
    }
}
