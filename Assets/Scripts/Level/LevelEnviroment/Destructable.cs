using UnityEngine;
public class Destructable : MonoBehaviour
{

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clip;
    [SerializeField] private GameObject _particle;
private void OnTriggerEnter2D(Collider2D other)
{
    if(other.gameObject.GetComponent<Spikes>()!=null)
    {
        GameObject particle = Instantiate(_particle, transform.position, Quaternion.identity);
        particle.transform.parent=null;
        source.PlayOneShot(clip[Random.Range(0, clip.Length)]);
        Destroy(gameObject);
    }
}
}
