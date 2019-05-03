using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinsTrigger : MonoBehaviour
{
    public GameController GameController;
    private void OnTriggerEnter(Collider other)
    {
        BallBehaviour ball;
        if ((ball = other.GetComponent<BallBehaviour>()) != null)
        {
            GameController.PinHit(null);
        }
    }
}
