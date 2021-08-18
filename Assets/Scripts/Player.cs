using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingAgent, IDamageble
{
    [SerializeField] private float _jumpForce;
     private bool _doubleJumped = false;

    [Header("Shooting")]
    [SerializeField] private Transform _gunMuzzle;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _fireRate = 2;
    private float _shootDelay = 0;

    // Movement
    private Vector2 _moveInput = Vector2.zero;
    private bool _jumpInput = false;

    [SerializeField] private LayerMask _groundMask;
    [SerializeField] protected bool isGrounded;
    [SerializeField] private Animator _animator;

    // jump squash
    [SerializeField] private Transform _visual;
    [SerializeField] private float _animTime = .5f;
    [SerializeField] private float _maxSquash = 1.2f, _minSquash = .8f;
    [SerializeField] private float _maxStretch = 1.2f, _minStretch = .8f;

    void Update()
    {
        // movement
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        _jumpInput = _jumpInput || Input.GetKeyDown(KeyCode.Space);

        // shooting
        if(Input.GetKey(KeyCode.X) && Time.time > _shootDelay)
        {
            Shoot();
        }

        _animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        _animator.SetBool("Grounded", isGrounded);
    }

    private void FixedUpdate()
    {
        Move(_moveInput, _jumpInput);
        _jumpInput = false;

        // grounded check
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rb.position, .2f, _groundMask);
        isGrounded = colliders.Length > 0;
    }

    private void Move(Vector2 speed, bool jump)
    {
        if (jump)
        {
            if (isGrounded || !_doubleJumped)
            {
                _doubleJumped = !isGrounded && !_doubleJumped;
                isGrounded = false;
                // reset Y velocity
                var v = rb.velocity;
                v.y = 0;
                rb.velocity = v;
                rb.AddForce(Vector2.up * _jumpForce);
            }
        }

        // calculate the target velocity
        Vector2 targetVelocity = new Vector2(speed.x * _speedModifier, rb.velocity.y);
        base.Move(targetVelocity);
    }

    private void Shoot()
    {
        _shootDelay = Time.time + 1 / _fireRate;
        Instantiate(_bulletPrefab, _gunMuzzle.position, Quaternion.identity).MoveRight = facingRight;
    }

    public void GetHit()
    {
        Reset();
    }

    private void Reset()
    {
        transform.position = new Vector3(.4f, 0);
    }
}
