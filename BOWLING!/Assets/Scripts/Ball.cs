using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _throw = false;
    
    /// <summary>
    /// Позиция к которой будет стремиться шар
    /// </summary>
    private Vector3 _ballPosition;
    
    public bool MouseDown;
    /// <summary>
    /// Максимальный поворот по модулю
    /// </summary>
    public float AbsMaxRotation;
    private float _strength;

    public float Strength
    {
        get
        {
            return _strength;
        }
        set
        {
            _strength = Mathf.Clamp(value,minStrength,maxStrength);
        }
    }

    public Vector3 startPosition;
    
    public float minStrength;
    public float maxStrength;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ballPosition = startPosition;
    }
    
    /// <summary>
    /// Возвращает шар на начальную позицию
    /// </summary>
    public void ToStart()
    {
        _throw = false;
        transform.position = startPosition;
        transform.localEulerAngles = Vector3.forward;
        _ballPosition = startPosition;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    public void Throw()
    {
        _throw = true;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(new Vector3(transform.forward.x * Strength, 0, transform.forward.z * Strength), ForceMode.VelocityChange);
    }

    public void RollTo(Vector3 position)
    {
        _ballPosition = position;
    }
    
    void Update()
    {    
        if (!_throw)
        {
            transform.position = Vector3.Lerp(transform.position, _ballPosition, 5*Time.deltaTime);
        }
    }

    private void OnMouseDown()
    {
        MouseDown = true;
    }
    private void OnMouseUp()
    {
        MouseDown = false;
    }
    

}

