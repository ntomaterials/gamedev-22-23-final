using System;
using System.Collections;
using UnityEngine;

public class ArcShooter : Weapon
{
    [Header("ArcBow")][Space(5)][SerializeField] private Transform firePoint;
    [SerializeField] private float distanceFromFirePoint = 0.3f;
    [SerializeField] private int shots=6;
    [SerializeField] private float arc = 180f;
    [SerializeField] private float arcOffset=0f;
    [SerializeField] private float timeForAttack=1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private AudioClip shotSound;
    
    public override void Fire()
    {
        StartCoroutine(StartArcAttack());
    }

    private IEnumerator StartArcAttack()
    {
        float arcRad = arc * Mathf.Deg2Rad;
        
        Vector3 dir = Vector2.zero;
        Quaternion rot;
        float angle = 0f;
        float startAngle = -arcRad / 2;
        
        for (int i = 0; i <= shots; i++)
        {
            angle = arcRad * ((float)i / (float)shots) + startAngle + transform.rotation.eulerAngles.z * Mathf.Deg2Rad + arcOffset * Mathf.Deg2Rad;
            
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            
            Vector2 dif = new Vector2(x, y) * distanceFromFirePoint;

            Instantiate(projectilePrefab, firePoint.position + (Vector3) dif, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));
            
            if(shotSound!=null) audioSource.PlayOneShot(shotSound);
            
            yield return new WaitForSeconds(timeForAttack / shots);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(firePoint.position, distanceFromFirePoint);
        
        float arcRad = arc * Mathf.Deg2Rad;
        
        Vector3 dir = Vector2.zero;
        Quaternion rot;
        float angle = 0f;
        float startAngle = -arcRad / 2 + transform.rotation.eulerAngles.z * Mathf.Deg2Rad + arcOffset * Mathf.Deg2Rad;
        
        for (int i = 0; i <= shots; i++)
        {
            angle = arcRad * ((float)i / (float)shots) + startAngle;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            Vector2 dif = new Vector2(x, y) * distanceFromFirePoint;
            
            Gizmos.DrawRay(firePoint.position + (Vector3)dif,   (Vector3)dif);
        }
        
    }
    
}
