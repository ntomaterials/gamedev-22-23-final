using System;
using System.Collections;
using UnityEngine;
public class SpiningCube : MonoBehaviour
{
    [SerializeField] private float spinReload;
    private float timeBtwSpin;
    [SerializeField] private float spinDuration;
    [SerializeField] private bool spinLeft=false;
    //[SerializeField] private float activeRadius;
    private bool isSpining;
    private void Start()
    {
        timeBtwSpin = spinReload;
        isSpining = false;
    }
    private void Update()
    {
        if (!isSpining && timeBtwSpin > 0) timeBtwSpin -= Time.deltaTime;
        if (timeBtwSpin <= 0)
        {
            StartCoroutine(Spining());
        }
    }
    private IEnumerator Spining()//не совсем понимаю, как это работает, взял с форума
    {
        isSpining = true;
        int dir = 1;
        if (!spinLeft) dir = -1;
        var startTime = Time.time;
        var startRotation = transform.localRotation;
        transform.Rotate(transform.forward, 45*dir);//если будет 90, повернется на 180
        var endRotation = transform.localRotation;
        float k = 0;
        while (k<1)
        {
            k = (Time.time - startTime) / spinDuration*2;//аналогично
            if (k > 1) k=1;//без этого накапливается ошибка
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, k);
            yield return null;
        }
        transform.localRotation = endRotation;
        isSpining = false;
        timeBtwSpin = spinReload;//не люблю возню с углами)
    }
}
