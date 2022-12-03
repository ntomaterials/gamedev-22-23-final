using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [field: SerializeField] public List<DoorLever> doorLevers { get; private set; }
    [field: SerializeField] public List<SaveFire> saveFires { get; private set; }
    public int lastFireID;
    [HideInInspector] public int id { get; set; }
    private Player player;
    private void Awake()
    {
        for (int i = 0; i<saveFires.Count; i++)
        {
            saveFires[i].id = i;
        }
        player = FindObjectOfType<Player>();
    }
    public void LoadLevelObjects(List<bool> leversStates)
    {
        player.transform.position = saveFires[lastFireID].playerSpawn.position;

        if (leversStates == null) // Если новая игра, все двери закрыты
        { 
            leversStates = new List<bool>();
            for (int i=0; i<doorLevers.Count; i++)
            {
                leversStates.Add(false);
            }
        }
        for(int i=0; i<doorLevers.Count; i++)
        {
            DoorLever lever = doorLevers[i];
            lever.IsOpen = leversStates[i];
            lever.CheckOpen();
        }
    }
}
