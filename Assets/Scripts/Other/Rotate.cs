using System;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 20f;

    private void Update()
    {
        Vector3 rot = new Vector3(0, 0, speed * Time.deltaTime);
        transform.Rotate(rot, Space.Self);
    }
}
