using UnityEngine;
public class Cudesnik : DialogWithInteractable
{
    private float appearDistance;
    
    private bool _appear = false;
    protected void Start()
    {
        base.Start();
        appearDistance = _collider.size.x / 2;
    }

    private void FixedUpdate()
    {
        float dist = (Player.Instance.transform.position - transform.position).magnitude;
        if (!_appear)
        {
            if (dist <= appearDistance)
            {
                _animator.SetTrigger("appear");
                _appear = true;
            }
        }
    }
}
