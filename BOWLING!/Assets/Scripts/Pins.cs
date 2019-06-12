using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour
{
    private List<Pin> _pins;

    public float liftSpeed;
    
    private bool _liftUp;
    private bool _liftDown;
    /// <summary>
    /// поднимает кегли к 0.75 по Y и отключает коллизию и гравитацию
    /// </summary>
    public void LiftUp()
    {
        _liftDown = false;
        _liftUp = true;
        Collide(false);
    }
    /// <summary>
    /// опускает кегли к 0 по Y и отключает коллизию и гравитацию
    /// </summary>
    public void LiftDown()
    {
        _liftDown = true;
        _liftUp = false;
    }
    
    private Vector3 _liftUpPosition;
    private Vector3 _liftDownPosition;
    
    void Start()
    {
        _liftDownPosition = transform.position;
        _liftUpPosition = _liftDownPosition;
        _liftUpPosition.y += 0.75f;
        
        _pins = new List<Pin>();
        foreach (Transform pin in transform)
        {
            _pins.Add(pin.GetComponent<Pin>());
        }
    }
    
    void Update()
    {
        for(int i = _pins.Count - 1; i >= 0; i--)
        {
            if (_pins[i].PinHit) continue;
            
            
            if (Mathf.Abs(_pins[i].transform.rotation.x) >= 0.2 || Mathf.Abs(_pins[i].transform.rotation.z) >= 0.2)
            {
                GameController.Instance.PinHit(_pins[i]);
                _pins[i].transform.parent = null;
                _pins[i].PinHit = true;
            }
        }

        if (_liftUp)
        {
            transform.position = Vector3.Lerp(transform.position,_liftUpPosition,liftSpeed*Time.deltaTime);
        }

        if (_liftDown)
        {
            transform.position = Vector3.Lerp(transform.position,_liftDownPosition,liftSpeed*Time.deltaTime);
        }
        
    }

    public void Restart()
    {
        _liftDown = false;
        _liftUp = false;
        Collide(true);
        
        foreach (Pin pin in _pins)
        {
            pin.PinHit = false;
            pin.Restart();
            pin.transform.parent = transform;
        }
    }

    public void Strike()
    {
        foreach (Pin pin in _pins)
        {
            if(!pin.PinHit)
            GameController.Instance.PinHit(pin);
        }
    }
    
    /// <summary>
    /// Включает или выключает коллизию и гравитацию у всех несбитых кеглей
    /// </summary>
    /// <param name="collide"></param>
    public void Collide(bool collide)
    {
        foreach (Pin pin in _pins)
        {
            if (!pin.PinHit)
            {
                pin.Collide = collide;
            }
        }
    }
}
