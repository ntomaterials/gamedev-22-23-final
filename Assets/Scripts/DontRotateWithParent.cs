using UnityEngine;

public class DontRotateWithParent : MonoBehaviour
{
   private Transform parent;

   private void Awake()
   {
      parent = transform.parent;
   }

   private void Update()
   {
      transform.eulerAngles = -parent.transform.eulerAngles;
   }
}
