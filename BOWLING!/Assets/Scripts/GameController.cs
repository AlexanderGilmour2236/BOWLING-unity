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
            //_mousePressed = false;

            _mouseDownPosition = Input.mousePosition;
//            if (_mouseDownPosition.y < Screen.currentResolution.height / 3)
//            {
                _mousePressed = true;
//            }
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
            _mouseUpPosition = Input.mousePosition;
            if(_mouseUpPosition.y > _mouseDownPosition.y)
            {
                if (_mousePressed)
                {
                    if (_ballThrown == false)
                    {
                        _ballThrown = true;
                        // min - 3 max - 9;
                        ball.strength = Random.Range(3,9);
                        ball.Throw();
                    }
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

