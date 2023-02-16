using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] protected int loadingID;
    protected LevelsData levelsData;
    protected void Awake()
    {
        levelsData = FindObjectOfType<LevelsData>();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            LoadNextLevel();
        }
    }
    protected void LoadNextLevel()
    {
        levelsData.levelOnScene.enemyHolder.KillEnemies();
        levelsData.LoadLevel(loadingID);
    }
}
