using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour
{
    public GameController gameController;
    private List<Pin> _pins;
    
    public bool LiftUp;
    public bool LiftDown;
    
    void Start()
    {
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
            
            if (Mathf.Abs(_pins[i].transform.rotation.x) >= 0.4 || Mathf.Abs(_pins[i].transform.rotation.z) >= 0.4)
            {
                gameController.PinHit(_pins[i]);
                _pins[i].transform.parent = null;
                _pins[i].PinHit = true;
            }
        }

        if (LiftUp)
        {
            LiftDown = false;
            //
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
    
}
