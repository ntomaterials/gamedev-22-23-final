using UnityEngine;

[CreateAssetMenu(fileName = "NewFlyOverGroundState", menuName = "Custom/States/FlyOverGroundState")]
public class FlyOverGroundState : State
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float targetHeight=4f;
    [SerializeField] private LayerMask groundLayerMask;
    private Rigidbody2D _ownerRigidbody;
    public override void Init(Enemy enemy)
    {
        base.Init(enemy);
        _ownerRigidbody = owner.GetComponent<Rigidbody2D>();
    }
    
    public override void Run()
    {
        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, Vector2.down, targetHeight, groundLayerMask);
        if (hit.collider == null)
        {
           isFinished = true;
        }
        else
        {
            if (owner.canMove)
            {
                _ownerRigidbody.velocity = (Vector2.up * speed);
            }
        }
    }
}
