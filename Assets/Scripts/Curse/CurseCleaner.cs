using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CurseCleaner : MonoBehaviour
{
    [HideInInspector] public CurseCaster caster;
    private Collider2D _colider;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>() == null) return;
        caster.Clear();
        Destroy(this.gameObject);
    }
}
