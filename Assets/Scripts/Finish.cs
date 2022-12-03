using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private int loadingID;
    private LevelsData levelsData;
    private void Awake()
    {
        levelsData = FindObjectOfType<LevelsData>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer)
        {
            levelsData.loadingLevelID = loadingID;
            levelsData.LoadLevel();
            collision.gameObject.transform.position = Vector2.zero;
        }
    }
}
