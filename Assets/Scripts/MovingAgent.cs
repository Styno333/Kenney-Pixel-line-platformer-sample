using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingAgent : MonoBehaviour
{
    [SerializeField] protected float _speedModifier = 10;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;

    protected Rigidbody2D rb;
    private Vector3 _velocity = Vector3.zero;
    protected bool facingRight = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Move(Vector2 targetVelocity)
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        if(targetVelocity.x < 0 && facingRight)
        {
            Flip();
        }
        else if(targetVelocity.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
