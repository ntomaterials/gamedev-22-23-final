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
        if (Player.Instance.GetCursesStucksByType(caster.curseType) >= GlobalConstants.S)
        {
            _effect.enabled = false;
        }
        else
        {
            _effect.enabled = true;
        }
    }
}
