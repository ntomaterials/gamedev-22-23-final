using UnityEngine;

public class MagicCloudProjectile : Arrow
{
    [Header("Arc")] [SerializeField] private float forcePercent = 0.04f;
    [SerializeField] private float forceDuration = 1f;
    private float _durationLeft = 1f;
    protected override void Start()
    {
        base.Start();
        _durationLeft = forceDuration;
    }
    private void FixedUpdate()
    {
        if (_durationLeft > 0)
        {
            _durationLeft -= Time.fixedDeltaTime;
            Vector2 prevVelocity = rigidbody.velocity;
            rigidbody.velocity = rigidbody.velocity * (1 - forcePercent);
            Vector2 dif = prevVelocity - rigidbody.velocity;
            rigidbody.velocity = rigidbody.velocity + (Vector2)transform.up * dif.magnitude;
        }
        
    }
}
