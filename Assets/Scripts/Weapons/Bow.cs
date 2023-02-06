using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Arrow projectilePrefab;
    [SerializeField] private AudioClip shotSound;
    public override void Fire()
    {
        if (reloadTime > 0f) return;
        if(shotSound!=null) audioSource.PlayOneShot(shotSound);
        reloadTime = reload;
        Arrow pr = Instantiate(projectilePrefab, firePoint.position, transform.parent.rotation);
        pr.damage = this.damage;//“ак будет удобнее настраивать урон стрел. ћожно еще к лучнику похожее написать
    }
}
