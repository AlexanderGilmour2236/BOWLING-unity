using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    private Rigidbody _rigidbody;
    private bool _throw = false;
    private bool _ballThrown = false;
    private bool _mousePressed = false;
    public float strength = 45;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        _throw = true;
        _ballThrown = true;
    }

    void Update()
    {

        if (_throw == true)
        {
            _throw = false;
            _rigidbody.AddForce(new Vector3(transform.forward.x * strength, 0, transform.forward.z * strength), ForceMode.VelocityChange);
            
        }
        if(_ballThrown == true)
        {
            
            transform.Rotate(transform.TransformDirection(new Vector3(-5.0f, 0, 0)));
        }
    }

    }

