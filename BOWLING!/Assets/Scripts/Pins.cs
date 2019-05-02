using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour
{
    public GameController gameController;
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
        for(int i = pins.Count - 1; i >= 0; i--)
        {
            if (Mathf.Abs(pins[i].transform.rotation.x) >= 0.4 || Mathf.Abs(pins[i].transform.rotation.z) >= 0.4)
            {
                gameController.PinHit(pins[i]);
                pins.Remove(pins[i]);
            }
        }
    }
}
