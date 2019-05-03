using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _startRotation;
    private Rigidbody _rigidbody;
    private bool _collide;
    /// <summary>
    /// вкл/выкл коллайдер и гравитацию
    /// </summary>
    public bool Collide
    {
        get { return _collide; }
        set
        {
            _collide = value;
            _rigidbody.isKinematic = !value;
            _rigidbody.detectCollisions = value;
            _rigidbody.useGravity = value;
        }
    }
    
    /// <summary>
    /// Сбита ли кегля
    /// </summary>
    public bool PinHit { get; set; }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _startRotation = transform.localEulerAngles;
    }

    /// <summary>
    /// Возвращает кеглю в начальную позицию
    /// </summary>
    public void Restart()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        
        transform.position = _startPosition;
        transform.localEulerAngles = _startRotation;
    }
    
    
}
