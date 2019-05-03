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
    public Vector3 startPosition;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// Возвращает шар на начальную позицию
    /// </summary>
    public void ToStart()
    {
        _throw = false;
        transform.position = startPosition;
        transform.localEulerAngles = Vector3.forward;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
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

