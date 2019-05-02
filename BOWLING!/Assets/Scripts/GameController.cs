using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public BallBehaviour ball;

    public Pins pins;
    public List<Pin> HitPins;
    
    private bool _ballThrown;
    
    private Coroutine _endTurnCorutine;
    private bool _endTurn;
    public GameObject pinsCleaner;
    
    private Vector3 _ballDirection;
    
    private bool _mousePressed;
    private Vector3 _mouseWorldPosition;
    private Vector2 _mouseDownPosition;
    private Vector2 _mouseUpPosition;

    private DateTime _mouseDownTime;
    private DateTime _mouseUpTime;
    
    public float minBallSpeed = 3;
    public float maxBallSpeed = 10;
    
    public float maxSlideTime = 500;

    private Animator _animator;
    private void Start()
    {
        HitPins = new List<Pin>();
        _animator = pinsCleaner.GetComponent<Animator>();
    }

    /// <summary>
    /// Возвращает шар в начальную позицию или возвращает кегли в начальные позиции
    /// </summary>
    public void Restart()
    {
        if (_ballThrown == false)
        {
            pins.Restart();
        }
        _ballThrown = false;
        ball.ToStart();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Restart();
        }
        
        // при нажатии левой кнопки мыши запоминаются координаты и время нажатия
        if (Input.GetMouseButtonDown(0))
        {

            _mouseDownTime = DateTime.Now;
            _mouseDownPosition = Input.mousePosition;
            _mousePressed = true;

        }
        
        // при зажатой левой кнопке миши шар выполняет поворот относительно курсора
        if (Input.GetMouseButton(0))
        {

            if (_mousePressed)
            {
                _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, pins.gameObject.transform.position.z));
                ball.transform.LookAt(new Vector3(_mouseWorldPosition.x, 0, 0));
            }
        }
        
        // при отжатии левой кнопки мыши высчитывается сила броска и выполняется функция ball.throw()
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

                        if (deltaTime >= maxSlideTime)
                        {
                            strength = minBallSpeed;
                        }
                        else
                        {
                            // deltatime от 0 до maxSlideTime-1 делится на максимальное время свайпа для броска
                            // получая величину от ~0 до 1, на которую делится сила броска
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


//        if (_endTurn == true)
//        {
//            pins.LiftUp = true;
//        }
    }

    /// <summary>
    /// Запускает EndTurn() для сбитой кегли, останавливает предыдущую корутину если она была запущена
    /// </summary>
    /// <param name="pin">Сбитая кегля</param>
    public void PinHit(Pin pin)
    {

        if (_endTurnCorutine != null)
        {
            StopCoroutine(_endTurnCorutine);
        }
        _endTurnCorutine = StartCoroutine(EndTurn());
        
        HitPins.Add(pin);
    }

    /// <summary>
    /// Отсчитывает одну секунду и объявляет об окончании хода 
    /// </summary>
    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("END");
        pins.LiftUp = true;
        _endTurn = true;
        yield return  new WaitForSeconds(1);
        _animator.Play("Clean");
    }
    
}

