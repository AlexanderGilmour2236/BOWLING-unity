using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHitTrigger : MonoBehaviour
{
    public GameController GameController;
    private void OnTriggerEnter(Collider other)
    {
        Ball ball;
        if ((ball = other.GetComponent<Ball>()) != null)
        {
            GameController.PinHit(null);
        }
    }
}
