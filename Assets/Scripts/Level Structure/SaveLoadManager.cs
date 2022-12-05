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
        //че-то там подготавливаем, заводим машину
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);
        Save save = new Save();
        //сохраняем и выключаем машину
        save.SavedLevelID = levelsData.lastSavedLevelID;
        save.SavedFireID = fireID;
        save.SaveLeversState(levelsData.levelOnScene.doorLevers);/////

        bf.Serialize(fs, save);
        fs.Close();
        Debug.Log("Saved");
    }
    //=====================================
    public void LoadData()
    {
        SetPath();
        //если файл существует, заводим машину
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
        levelsData.LoadLevel(save.SavedLevelID);
        levelsData.levelOnScene.lastFireID = save.SavedFireID;
        levelsData.levelOnScene.LoadLevelObjects(save.leversStates);
    }
    public void NewGame()
    {
        Debug.Log("New Game");
        levelsData.LoadLevel(0);
        levelsData.levelOnScene.lastFireID = 0;
        levelsData.levelOnScene.LoadLevelObjects(null);
    }
    [System.Serializable]
    public class Save
    {
        public int SavedLevelID;
        public int SavedFireID;
        public List<bool> leversStates=new List<bool>();
        public void SaveLeversState(List<DoorLever> levers)
        {
            foreach (var lever in levers)
            {
                leversStates.Add(lever.IsOpen);
            }
        }
    }
}
