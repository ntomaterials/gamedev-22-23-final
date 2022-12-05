using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    public void Attack()
    {
        if (reloadTime > 0f) return;
        reloadTime = reload;
        GameObject pr = Instantiate(projectilePrefab, firePoint.position, transform.parent.rotation);
    }
    
}
