using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float reload = 3f;
    private float _reloadTime = 0f;

    public void Attack()
    {
        if (_reloadTime > 0f) return;
        _reloadTime = reload;
        GameObject pr = Instantiate(projectilePrefab, firePoint.position, transform.parent.rotation);
    }

    private void Update()
    {
        _reloadTime -= Time.deltaTime;
    }
    public bool ready
    {
        get { return _reloadTime <= 0; }
    }
}
