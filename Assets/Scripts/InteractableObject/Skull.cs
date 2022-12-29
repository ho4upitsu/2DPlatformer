using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    [SerializeField] private int _heal;
    [SerializeField] private float _healDelay;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _redSkull;
    [SerializeField] private Sprite _skull;
 
    private float _lastHealTime;
    private PlayerMover _playerMover;
    private bool toched = false;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        _playerMover = collider.GetComponent<PlayerMover>();
        if (_playerMover != null)
        {
            _playerMover.Heal(_heal);
            _lastHealTime = Time.time;
            toched = true;
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        if (toched)
        {
            _spriteRenderer.sprite = _redSkull; 
        }
        else
        {
            _spriteRenderer.sprite = _skull;
        }
    }

    private void Update()
    {
        if (Time.time - _lastHealTime > _healDelay && _playerMover != null)
        {
            _playerMover.Heal(_heal);
            _lastHealTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        PlayerMover playerMover = collider.GetComponent<PlayerMover>();
        if (_playerMover == playerMover)
        {
            _playerMover = null;
            toched = false;
            ChangeSprite();
        }
    }
}