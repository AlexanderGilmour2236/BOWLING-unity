using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinsDownTrigger : MonoBehaviour
{
    public GameController GameController;
    private void OnTriggerEnter(Collider other)
    {
        Pin pin;
        if ((pin = other.GetComponent<Pin>()) != null)
        {
            if (!pin.PinHit)
            {
                GameController.PinHit(pin);
            }
        }
    }
}
