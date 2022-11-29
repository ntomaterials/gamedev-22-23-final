using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("X moving")]
    [SerializeField] private float Xamplitude;
    [SerializeField] private float Xvelocity;
    
    [Header("Y moving")]
    [SerializeField] private float Yamplitude;
    [SerializeField] private float Yvelocity;

    private float Xoffset;
    private float Yoffset;
    private float time;
    private Vector2 startPos;
    private void Awake()
    {
        startPos = transform.position;
    }
    private void Update() 
    {
        time = time + Time.deltaTime;
        Xoffset = Xamplitude * Mathf.Sin(time * Xvelocity);
        Yoffset = Yamplitude * Mathf.Sin(time * Yvelocity);
        transform.position = new Vector2(startPos.x+Xoffset, startPos.y+Yoffset);
    }
    private void OnDrawGizmos()
    {
        try
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Gizmos.color = Color.blue;
            float width = Xamplitude * renderer.size.x + renderer.size.x;
            if (Xamplitude == 0) width = 0.2f;
            float height = Yamplitude * renderer.size.y + renderer.size.y;
            if (Yamplitude == 0) height = 0.2f;
            Vector2 center = startPos;
            if (startPos == null) center = transform.position;
            Gizmos.DrawWireCube(center, new Vector3(width, height, 0));
        }
        catch
        {
            return;
        }
    }
}
