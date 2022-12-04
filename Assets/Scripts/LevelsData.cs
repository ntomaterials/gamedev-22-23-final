using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsData : MonoBehaviour
{
    [field: SerializeField] public List<Level> allLevels { get; private set; }
    [HideInInspector] public int loadingLevelID; //�������, ������� �� ����� ���������
    public Level levelOnScene { get; private set; }//�������, ������� ������ �������� �� �����
    private SaveLoadManager saveLoadManager;
    private void Awake()
    {
        for (int i=0; i<allLevels.Count; i++)
        {
            allLevels[i].id = i;
        }
        //if(levelOnScene=null) levelOnScene=allLevels[0]; // ����� ������ �� ���� (� ��� ��� ����� �����)
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        saveLoadManager.levelsData = this;
        Debug.Log(saveLoadManager.levelsData);
        saveLoadManager.LoadData();
    }
    public void LoadLevel()
    {
        if (levelOnScene != null)
        {
            if (levelOnScene != allLevels[loadingLevelID])
            {
                Destroy(levelOnScene.gameObject);
                Loading();
            }
        }
        else Loading();
    }
    private void Loading()
    {
       levelOnScene= Instantiate(allLevels[loadingLevelID].gameObject, Vector2.zero, Quaternion.identity).GetComponent<Level>();
        levelOnScene.id = loadingLevelID;
    }
}
