using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour
{

    public List<GameObject> pins;
    
    void Start()
    {
        foreach (Transform pin in transform)
        {
            pins.Add(pin.gameObject);
        }
    }

    void Update()
    {
        
    }
}
