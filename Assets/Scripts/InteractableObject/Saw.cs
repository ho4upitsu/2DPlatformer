using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _damageDelay;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _bloodySaw;
 
    private float _lastDamageTime;
    private PlayerMover _playerMover;
    private bool toched = false;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        _playerMover = collider.GetComponent<PlayerMover>();
        if (_playerMover != null)
        {
            _playerMover.TakeDamage(_damage);
            _lastDamageTime = Time.time;
            toched = true;
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        if (toched)
        {
            _spriteRenderer.sprite = _bloodySaw; 
        }
    }

    private void Update()
    {
        if (Time.time - _lastDamageTime > _damageDelay && _playerMover != null)
        {
            _playerMover.TakeDamage(_damage);
            _lastDamageTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        PlayerMover playerMover = collider.GetComponent<PlayerMover>();
        if (_playerMover == playerMover)
        {
            _playerMover = null;
        }
    }
}