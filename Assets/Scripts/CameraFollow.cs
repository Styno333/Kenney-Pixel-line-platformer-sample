using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Camera _camera;
    [SerializeField] private float _dampTime = 0.15f;
    private Vector3 _velocity = Vector3.zero;
    public Transform Target;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target)
        {
            Vector3 point = _camera.WorldToViewportPoint(Target.position);
            Vector3 delta = Target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); 
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, _dampTime);
        }

    }
}
