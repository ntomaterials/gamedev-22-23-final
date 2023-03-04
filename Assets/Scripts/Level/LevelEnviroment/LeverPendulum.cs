using UnityEngine;
public class LeverPendulum : DoorLever
{
[SerializeField] private GameObject falseObj;
[SerializeField] private GameObject trueObj;
    protected override void Awake()
    {
        base.Awake();
        falseObj.SetActive(true);
        trueObj.SetActive(false);
    }
    public override void CheckOpen()
    {
        if (IsOpen) 
        {
            falseObj.SetActive(false);
            trueObj.SetActive(true);
            spriteRenderer.sprite=Opened;
        }
        else spriteRenderer.sprite=Closed;
        audioSource.PlayOneShot(leverSound);
    }
}
