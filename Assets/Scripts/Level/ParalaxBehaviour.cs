using UnityEngine;
public class ParalaxBehaviour : MonoBehaviour
{
    [SerializeField] private Transform folowingTarget;
    [SerializeField, Range(0f, 1f)] private float paralaxStrength = 0.1f;
    [SerializeField] private bool disableVerticalParalax;
    private Vector3 targetPreviousPosition;
    private void Start()
    {
        if (!folowingTarget) folowingTarget = Camera.main.transform;
        targetPreviousPosition = folowingTarget.position;
    }
    private void Update()
    {
        Vector3 delta = folowingTarget.position - targetPreviousPosition;
        if (disableVerticalParalax) delta.y = 0;
        targetPreviousPosition = folowingTarget.position;
        transform.position += delta * paralaxStrength;
    }
}
