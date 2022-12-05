using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class CurseCleaner : MonoBehaviour
{
    [SerializeField] private float radius=3;
    [SerializeField] private float angle=30f;
    [HideInInspector] public CurseCaster caster;
    private VisualEffect _effect;
    private bool _isPlaying=true;
    private void Awake()
    {
        _effect = GetComponent<VisualEffect>();
        _effect.SetFloat("Arc", angle * Mathf.Deg2Rad);
        _effect.SetFloat("Radius", radius);
        _effect.SetFloat("Rotation", angle / 2);
    }

    private void LateUpdate() 
    {
        //Awake();
        Vector3 dist = Player.Instance.transform.position - transform.position;
        dist = -dist;
        if (dist.magnitude <= radius)
        {
            Vector3 dir = (transform.position - Player.Instance.transform.position).normalized;
            float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            a += 90f;
            if (a < 0)
            {
                a += 360f;
            }
            //print($"a{a}, zrot{transform.rotation.eulerAngles.z}, {a - transform.rotation.eulerAngles.z}");
            if (Mathf.Abs(a - transform.rotation.eulerAngles.z) <= angle / 2) // если в конусе
            {
                Player.Instance.ClearCursesFromCaster(caster);
            }
        }
    }

    private void Update()
    {
        if (Player.Instance.GetCursesStucksByType(caster.curseType) >= CursesManager.Instance.S)
        {
            _effect.enabled = false;
        }
        else
        {
            _effect.enabled = true;
        }
        float dist = (Player.Instance.transform.position - caster.transform.position).magnitude;
        if (dist > caster.radius * 1.5f)
        {
            if (_isPlaying)
            {
                _effect.Stop();
                _isPlaying = false;
            }
        }
        else
        {
            if (!_isPlaying)
            {
                _isPlaying = true;
                _effect.Play();
            }
        }
    }
    
}
