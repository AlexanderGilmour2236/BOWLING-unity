using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class BallBehaviour : MonoBehaviour
{
    //[SerializeField]
    //private Camera _camera;
    private Rigidbody _rigidbody;
    private bool _throw = false;
    
    public float strength = 5;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        _throw = true;
    }

    void Update()
    {    
        if (_throw == true)
        {
            _throw = false;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(new Vector3(transform.forward.x * strength, 0, transform.forward.z * strength), ForceMode.VelocityChange);
            
        }

    }

    }

