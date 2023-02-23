using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Teleport : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToPort;
    [SerializeField] private Transform _spawnPoint;
    private Collider2D _collider;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            //if ((_layersToPort.value & (1 << collision.gameObject.layer))==1)
            if(collision.gameObject.layer==GlobalConstants.PlayerLayer)
            {
                Debug.Log("Hoba!");
                collision.transform.position = _spawnPoint.position;
            }
        }
    }