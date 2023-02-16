using UnityEngine;
public abstract class ToActiveCycle : MonoBehaviour
{
    public bool isActive;
    protected virtual void Start()
    {
        isActive = false;
    }
    public virtual void StartDo() { }
    public virtual void StopDo() { }
}