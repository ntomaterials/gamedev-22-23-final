using UnityEngine;

public class DestroyOnPC : MonoBehaviour
{
    [SerializeField] private bool editor=true;
    [SerializeField] private bool standalone = true;
    private void Awake()
    {
        # if UNITY_STANDALONE
        if (standalone)
        {
            Destroy(this.gameObject);
        }
        #endif
        # if UNITY_EDITOR
            if (editor)
            {
                Destroy(this.gameObject);
            }
        #endif
    }
}
