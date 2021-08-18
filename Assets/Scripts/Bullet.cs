using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovingAgent
{
    public bool MoveRight;
    [SerializeField] private float _maxTimeAlive;
    private float _deathTime;

    private void Start()
    {
        _deathTime = Time.time + _maxTimeAlive;
    }

    private void Update()
    {
        if(Time.time > _deathTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = (MoveRight ? Vector2.right : -Vector2.right) * _speedModifier;
        Move(targetVelocity);
    }
}
