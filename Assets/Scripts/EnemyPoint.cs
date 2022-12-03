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
        if(myEnemy!=null)
        {
            Destroy(myEnemy.gameObject);
        }
        myEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
    }
}
