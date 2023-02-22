using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class Butch : MonoBehaviour
{
    [SerializeField] private Product _product;
    [SerializeField] private int _cost;
    [SerializeField] private AudioClip _buySound;
    private Collider2D _collider;
    private bool _canTryBuy;
    private Player _player;
    private PlayerInventory _inventory;
    private AudioSource _audioSource;
    private void Start()
    {
        InputHandler.Instance.onActionBtnUp += BuyMeet;
        _player = Player.Instance;
        _inventory = Player.Inventory;
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _canTryBuy = false;
        _audioSource = GetComponent<AudioSource>();
    }
    private void BuyMeet()
    {
        if (_canTryBuy)
        {
            if (_player.playerXp >= _cost && _inventory.CanAdd())
            {
                _player.SpendXp(_cost);
                _inventory.AddProduct(_product);
                _audioSource.PlayOneShot(_buySound);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) _canTryBuy = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GlobalConstants.PlayerLayer) _canTryBuy = false;
    }
}
