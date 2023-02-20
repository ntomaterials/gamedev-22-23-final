using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class MagicCloud : MonoBehaviour
{
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private Quaternion[] rotations;
    [SerializeField] private float shootDelay = 1f;

    private void Start()
    {
        StartCoroutine(SpawnProjectiles());
    }

    private IEnumerator SpawnProjectiles()
    {
        foreach (var rotation in rotations)
        {
            yield return new WaitForSeconds(shootDelay);
            Instantiate(projectiles[Random.Range(0, projectiles.Length)], transform.position, rotation);
        }
        Destroy(this.gameObject);
    }
}
