using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public BallBehaviour ball;
    public Transform Skeetles;
    public TrailRenderer Trail;

    private bool BallThrown = false;
    private bool _mousePressed;

    private Vector3 _ballDirection;



    private Vector3 _mouseWorldPosition;

    private Vector2 _mouseDownPosition;
    private Vector2 _mouseUpPosition;

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void Update()
    {
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Skeetles.position.z));

        Trail.AddPosition(new Vector3(_mouseWorldPosition.x, _mouseWorldPosition.y, Camera.main.transform.position.z+0.1f));

        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _mousePressed = false;

            _mouseDownPosition = Input.mousePosition;
            if (_mouseDownPosition.y < Screen.currentResolution.height / 3)
            {
                _mousePressed = true;
            }
        }

        if (Input.GetMouseButton(0))
        {

            if (_mousePressed)
            {
                _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Skeetles.position.z));
                ball.transform.LookAt(new Vector3(_mouseWorldPosition.x, 0, 0));
                Debug.Log("11");
            }



        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseUpPosition = Input.mousePosition;
            if(_mouseUpPosition.y > Screen.currentResolution.height / 3)
            {
                if (_mousePressed)
                {
                    if (BallThrown == false)
                    {
                        BallThrown = true;
                        ball.Throw();
                    }
                }

            }

            _mousePressed = false;
        }
    }
}

