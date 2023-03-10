using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    public float timeLeft = 10f;
    
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
