using System;
using UnityEngine;

public class WarningBeforeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float timeBeforeAttack = 2f;
    private float _timeBeforeAttackLeft;

    private void Awake()
    {
        _timeBeforeAttackLeft = timeBeforeAttack;
    }

    private void Update()
    {
        _timeBeforeAttackLeft -= Time.deltaTime;
        if (_timeBeforeAttackLeft <= 0f)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        Destroy(this.gameObject);
    }
}
