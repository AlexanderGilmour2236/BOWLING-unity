using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public BallBehaviour ball;

    public Pins pins;

    private bool _ballThrown = false;
    private bool _mousePressed;

    private Vector3 _ballDirection;

    private Vector3 _mouseWorldPosition;

    private Vector2 _mouseDownPosition;
    private Vector2 _mouseUpPosition;

    public float minBallSpeed = 3;
    public float maxBallSpeed = 10;
    
    private DateTime _mouseDownTime;
    private DateTime _mouseUpTime;

    public float maxSlideTime = 500;
    
    private void Start()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetMouseButtonDown(0))
        {

            _mouseDownTime = DateTime.Now;
            _mouseDownPosition = Input.mousePosition;
            _mousePressed = true;

        }
        
        
        if (Input.GetMouseButton(0))
        {

            if (_mousePressed)
            {
                _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, pins.gameObject.transform.position.z));
                ball.transform.LookAt(new Vector3(_mouseWorldPosition.x, 0, 0));
            }



        }
        
        if (Input.GetMouseButtonUp(0))
        {                    
            _mouseUpTime = DateTime.Now;
            _mouseUpPosition = Input.mousePosition;
            
            if(_mouseUpPosition.y > _mouseDownPosition.y)
            {

                    if (_ballThrown == false)
                    {
                        _ballThrown = true;

                        //Screen.height = 100%
                        //SlideLength = x%

                        float screenPercent = (_mouseUpPosition.y - _mouseDownPosition.y) * 100 / Screen.height;
                        
                        // maxBallSpeed = 100%
                        // x = screenPercent

                        float strength = screenPercent * maxBallSpeed / 100;
                        
                        float deltaTime = (float)(_mouseUpTime - _mouseDownTime).TotalSeconds;

                        if (deltaTime > maxSlideTime)
                        {
                            strength = minBallSpeed;
                        }
                        else
                        {
                            float timeEffect =
                            deltaTime / maxSlideTime;

                            strength /= timeEffect;
                        }
                        
                        strength = Mathf.Clamp(strength, minBallSpeed, maxBallSpeed);
                        
                        ball.strength = strength;
                        ball.Throw();
                    }

            }

            _mousePressed = false;
        }

//        foreach(Skittle s in skittles)
//        {
//
//            if (Mathf.Abs(s.transform.localRotation.z) > 90)
//            {
//                
//                s.gameObject.SetActive(false);
//
//            }
//
//            if (Mathf.Abs(s.transform.localRotation.x) > 90)
//            {
//
//                s.gameObject.SetActive(false);
//
//            }
//        }

    }
}

