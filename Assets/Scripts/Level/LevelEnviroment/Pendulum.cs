using UnityEngine;
public class Pendulum : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float amp = 30;
    private void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Sin(Time.time * speed) * amp));
    }
}
