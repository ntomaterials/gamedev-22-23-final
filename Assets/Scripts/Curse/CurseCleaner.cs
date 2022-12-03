using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider2D))]
public class CurseCleaner : MonoBehaviour
{
    [SerializeField] private GameObject cleaner;
    [SerializeField] private LayerMask cleanerCantSpawnMask;
    [HideInInspector] public CurseCaster caster;
    private VisualEffect _effect;
    private Collider2D _collider;

    private void Awake()
    {
        _effect = GetComponent<VisualEffect>();
        _collider = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        if (_collider.bounds.Contains(Player.Instance.transform.position)) Player.Instance.ClearCursesFromCaster(caster);
    }

    private void Update()
    {
        float dist = (Player.Instance.transform.position - caster.transform.position).magnitude;
        if (Player.Instance.GetCursesStucksByType(caster.curseType) >= CursesManager.Instance.S || dist > caster.radius * 1.5f)
        {
            _effect.enabled = false;
        }
        else
        {
            _effect.enabled = true;
        }
    }
}
