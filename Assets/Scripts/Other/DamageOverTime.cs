using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageOverTime : MonoBehaviour
{
    private Collider2D _collider;
    [SerializeField] private int damage = 3;
    [SerializeField] private float repeatRate = 0.5f;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        InvokeRepeating("CheckPlayer", 0f, repeatRate);
    }

    private void CheckPlayer()
    {
        if (_collider.bounds.Contains(Player.Instance.transform.position))
        {
            Player.Instance.GetDamage(damage);
        }
    }
}
