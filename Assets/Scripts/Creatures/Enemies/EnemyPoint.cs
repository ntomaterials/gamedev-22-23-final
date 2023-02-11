using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Enemy myEnemy;
    private void Awake()
    {
        myEnemy = null;
        LoadEnemy();
    }
    public void LoadEnemy()
    {
        Kill();
        myEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
    }
    public void Kill()
    {
        if (myEnemy != null)
        {
            Destroy(myEnemy.gameObject);
        }
    }
}
