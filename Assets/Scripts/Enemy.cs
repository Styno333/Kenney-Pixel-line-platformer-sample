using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentMoveAxis
{
    Horizontal,
    Vertical
}

public class Enemy : MovingAgent, IDamageble
{
    [SerializeField] private AgentMoveAxis _moveAxis;
    [SerializeField] private float _halfMoveDistance;
    private Vector2 _direction;
    private Vector2 _startPos;

    protected override void Awake()
    {
        base.Awake();

        _startPos = transform.position;
        _direction = _moveAxis == AgentMoveAxis.Horizontal ? Vector2.left : Vector2.up;
    }

    private void Update()
    {
        // calculate distance in Update instead of FixedUpdate
        var towards = _startPos + _direction * _halfMoveDistance;
        if (Vector2.Distance(towards, transform.position) < 0.1f)
            _direction = -_direction;
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = _direction * _speedModifier;
        Move(targetVelocity);
    }

    public void GetHit()
    {
        // Die
        rb.AddForce(Vector2.up * 200);
        rb.gravityScale = 1;
        GetComponent<Collider2D>().enabled = false;
        var scale = transform.localScale;
        scale.y = -1;
        transform.localScale = scale;
        Destroy(gameObject, 2);
        this.enabled = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var towards = _startPos + _direction * _halfMoveDistance;
        Gizmos.DrawSphere(towards, .1f);

        Gizmos.color = Color.magenta;
        if(_moveAxis == AgentMoveAxis.Horizontal)
            Gizmos.DrawLine(transform.position + Vector3.left * _halfMoveDistance, transform.position + Vector3.right * _halfMoveDistance);
        else
            Gizmos.DrawLine(transform.position + Vector3.up * _halfMoveDistance, transform.position + Vector3.down * _halfMoveDistance);
    }
#endif
}
