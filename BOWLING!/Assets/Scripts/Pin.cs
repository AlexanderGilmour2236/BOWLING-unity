using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _startRotation;
    private Rigidbody _rigidbody;
    public bool PinHit { get; set; }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _startRotation = transform.localEulerAngles;
    }

    public void Restart()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        
        transform.position = _startPosition;
        transform.localEulerAngles = _startRotation;
    }
}
