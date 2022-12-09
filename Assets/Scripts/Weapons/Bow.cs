using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip shotSound;
    public override void Fire()
    {
        if (reloadTime > 0f) return;
        if(shotSound!=null) audioSource.PlayOneShot(shotSound);
        reloadTime = reload;
        GameObject pr = Instantiate(projectilePrefab, firePoint.position, transform.parent.rotation);
    }
}
