using UnityEngine;
    [RequireComponent (typeof(Animator))]
public class Cudesnik : MonoBehaviour
{
    
    private bool _appear = false;
    private Animator _animator;
    private void Start() {
        
        _animator=GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            if (!_appear)
            {
                _animator.SetTrigger("appear");
                _appear = true;
            }
        }
    }
    /*private void FixedUpdate()
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
    }*/
}
