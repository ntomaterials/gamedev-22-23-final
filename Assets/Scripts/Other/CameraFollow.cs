using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform=null;
    [SerializeField] private string playerTag="";
    [SerializeField] private float movingSpeed=0;
    [SerializeField] private float X_offset;
    [SerializeField] private float Y_offset;
    //[SerializeField] private Vector3 dif;
    private void LateUpdate()
    {
        if (this.playerTransform == null)
        {
            if (this.playerTag == "") this.playerTag = "Player";
            this.playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        }
        //if (Math.Abs(playerTransform.position.x - transform.position.x) >= dif.x || Math.Abs(playerTransform.position.y - transform.position.y) >= dif.y)
        this.transform.position = new Vector3(this.playerTransform.position.x+X_offset, this.playerTransform.position.y+Y_offset, this.playerTransform.position.z - 10);
    }
    private void Update()
    {
        if (this.playerTransform)
        {
            /*if (Math.Abs(playerTransform.position.x - transform.position.x) >= dif.x || Math.Abs(playerTransform.position.y - transform.position.y) >= dif.y)
            {

            }*/
            Vector3 target = new Vector3(this.playerTransform.position.x + X_offset, this.playerTransform.position.y + Y_offset, this.playerTransform.position.z - 10);
            Vector3 pos = Vector3.Lerp(this.transform.position, target, this.movingSpeed * Time.deltaTime);

            this.transform.position = pos;
        }
    }
}
