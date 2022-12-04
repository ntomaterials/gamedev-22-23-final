using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string filePath;
    private Save save = new Save();

    public LevelsData levelsData;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        filePath = Application.persistentDataPath + "/save.save";
        Debug.Log(filePath);
    }
    public void SaveData(int fireID)
    {
        //че-то там подготавливаем, заводим машину
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);

        //создаем экземпл€р
        Save save = new Save();
        //сохран€ем и выключаем машину
        save.SavedLevelID = levelsData.levelOnScene.id;
        save.SavedFireID = fireID;
        save.SaveLeversState(levelsData.levelOnScene.doorLevers);

        bf.Serialize(fs, save);
        fs.Close();
        Debug.Log("Saved");
    }
    //=====================================
    public void LoadData()
    {
        //если файл существует, заводим машину
        if (!File.Exists(filePath))
        {
            Debug.Log("No files?");
            NewGame();
            return;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        //десереализуем и выключаем
        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

        //======================
        levelsData.loadingLevelID= save.SavedLevelID;
        levelsData.LoadLevel();
        levelsData.levelOnScene.lastFireID = save.SavedFireID;
        levelsData.levelOnScene.LoadLevelObjects(save.leversStates);
    }
    public void NewGame()
    {
        Debug.Log("New Game");
        levelsData.loadingLevelID = 0;
        levelsData.LoadLevel();
        levelsData.levelOnScene.lastFireID = 0;
        levelsData.levelOnScene.LoadLevelObjects(null);
    }
    [System.Serializable]
    public class Save
    {
        //создаем свой вариант Vector 3
        [System.Serializable]
        public struct Vec3
        {
            public float X, Y, Z;
            public Vec3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        //==================================
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
