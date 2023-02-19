using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip shotSound;
    public override void Fire()
    {
        if(shotSound!=null) audioSource.PlayOneShot(shotSound);
        GameObject arrow = Instantiate(projectilePrefab, firePoint.position, transform.parent.rotation);
        arrow.GetComponent<Arrow>().damage = this.damage;//“еперь урон лука удобнее настроить из префаба лука (надо бы еще лучником похожее прикрутить)
    }
}
