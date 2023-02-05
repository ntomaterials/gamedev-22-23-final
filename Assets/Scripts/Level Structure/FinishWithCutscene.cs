using UnityEngine;
public class FinishWithCutscene : Finish
{
    [SerializeField] protected Cutscene cutscene;
    private void Start()
    {
        cutscene.onCutsceneEnded += LoadNextLevel;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            Debug.Log("pu-pu");
            cutscene.PlayCutscene();
        }
    }
}
