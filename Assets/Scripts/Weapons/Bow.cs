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
        Arrow arrow = Instantiate(projectilePrefab, firePoint.position, transform.parent.rotation).GetComponent<Arrow>();
        arrow.damage = this.damage;//“еперь урон лука удобнее настроить из префаба лука (надо бы еще лучником похожее прикрутить)
    }
}
