using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField] private List<EnemyPoint> enemiesInArea;
    public void ReloadEnemies()
    {
        foreach(var enemy in enemiesInArea)
        {
            enemy.LoadEnemy();
        }
    }
    public void KillEnemies()
    {
        foreach(var enemy in enemiesInArea)
        {
            QuestDeathMark mark = enemy.GetComponent<QuestDeathMark>();
            enemy.Kill();
        }
    }
}
