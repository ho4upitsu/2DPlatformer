using System;
using Newtonsoft.Json.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Vector2;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMover : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _maxHP;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _HPText;

    private int _currentHP;

    [Header("Movement")]
    [SerializeField] public Transform _Transform;
    [SerializeField] public Transform _GroundChecker;
    [SerializeField] public Rigidbody2D _Rigidbody;
    [SerializeField] public LayerMask _whatIsGround;
    
    
    [Header("Animation")]
    [SerializeField] public Animator _Animator;
    [SerializeField] public string _runAnimationKey;
    [SerializeField] public string _crouchAnimationKey;
    [SerializeField] public string _jumpAnimationKey;
    
    [SerializeField] public float _speed;
    [SerializeField] public float _jumpPower;
    [SerializeField] public float _groundCheckerRadius = 0.2f;

    [SerializeField] public Transform _cellChecker;
    [SerializeField] public float _cellCheckerRadius;

    [SerializeField] public Collider2D _headCollider;
    
    private bool _facingRight = true;

    private void Start()
    {
        _currentHP = _maxHP;
        _slider.maxValue = _maxHP;
        _slider.value = _currentHP;
        _HPText.text = _currentHP.ToString();
    }

    public void TakeDamage(int damage)
    { 
        _currentHP = _currentHP - damage;
        _slider.value = _currentHP;
        _HPText.text = _currentHP.ToString();
        if (_currentHP <= 20)
        {
            _HPText.color = Color.red;
        }
        if (_currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int heal)
        {
            if (_currentHP < 100)
            {
                _currentHP += heal;
            }
            _slider.value = _currentHP;
            _HPText.text = _currentHP.ToString();
            if (_currentHP >= 20)
            {
                _HPText.color = Color.white;
            }
        }

    private void Die()
    {
        SceneManager.LoadScene("proj");
    }
    private void Update()
    {
        var grounded = Physics2D.OverlapCircle(_GroundChecker.position, _groundCheckerRadius, _whatIsGround);
        
        float direction = Input.GetAxisRaw("Horizontal");
        Vector2 velocity = _Rigidbody.velocity;
        _Animator.SetBool(_runAnimationKey, direction != 0);
        
        if (grounded)
        {
            _Rigidbody.velocity = new Vector2(_speed * direction, velocity.y);
            
            if (direction < 0 && _facingRight)
            {
                Flip();
            }
            else if (direction > 0 && !_facingRight)
            {
                Flip();
            }
        }
        
        if (Input.GetButtonUp("Jump") && grounded)
        {
            _Rigidbody.velocity = new Vector2(velocity.x, _jumpPower);
        }

        _Animator.SetBool(_jumpAnimationKey, !grounded);
        

        bool cellAbove = Physics2D.OverlapCircle(_cellChecker.position, _cellCheckerRadius, _whatIsGround);
        
        if (Input.GetKey(KeyCode.C))
        {
            _headCollider.enabled = false;
        }
        else if (!cellAbove)
        {
            _headCollider.enabled = true;
        }
        
        _Animator.SetBool(_crouchAnimationKey, !_headCollider.enabled);
    }

    private void Flip()
    {
        _Transform.Rotate(0, 180, 0);
        _facingRight = !_facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_GroundChecker.position,_groundCheckerRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_cellChecker.position, _cellCheckerRadius);
    }
}
