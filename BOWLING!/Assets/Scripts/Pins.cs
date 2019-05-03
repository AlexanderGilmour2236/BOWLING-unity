using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour
{
    public GameController gameController;
    private List<Pin> _pins;
    
    private bool _liftUp;
    public bool _liftDown;
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
            
            if (Mathf.Abs(_pins[i].transform.rotation.x) >= 0.3 || Mathf.Abs(_pins[i].transform.rotation.z) >= 0.3)
            {
                gameController.PinHit(_pins[i]);
                _pins[i].transform.parent = null;
                _pins[i].PinHit = true;
            }
        }

        if (_liftUp)
        {
            transform.position = Vector3.Lerp(transform.position,_liftUpPosition,3*Time.deltaTime);
        }

        if (_liftDown)
        {
            transform.position = Vector3.Lerp(transform.position,_liftDownPosition,3*Time.deltaTime);
        }
        
    }

    public void Restart()
    {
        foreach (Pin pin in _pins)
        {
            pin.Restart();
            pin.transform.parent = transform;
            pin.PinHit = false;
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
