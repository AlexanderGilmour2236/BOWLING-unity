﻿using System;
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

    public CameraBehaviour mainCamera;
    
    private Animator _pinsCleanerAnimator;
    [SerializeField] private AnimationClip _pinsCleanerAnimationClip;

    public List<Player> players;
    private int CurrentPlayerIndex;
    private void Start()
    {
        HitPins = new List<Pin>();
        _pinsCleanerAnimator = pinsCleaner.GetComponent<Animator>();
    }

    public void GameStart()
    {
        if (players.Count < 1)
        {
            return;
        }
        CurrentPlayerIndex = 0;
    }    
    
    
    /// <summary>
    /// Возвращает кегли в начальные позиции
    /// </summary>
    public void RestartPins()
    {
        pins.Restart();
        HitPins.Clear();
    }
    /// <summary>
    /// Возвращает шар в начальные позиции
    /// </summary>
    public void RestartBall()
    {
        _ballThrown = false;
        ball.ToStart();
        mainCamera.target = ball.transform;
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            RestartPins();
            RestartBall();
        }

        if (!_endTurn && !_ballThrown)
        {
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
                    _mouseWorldPosition = mainCamera.Camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, pins.gameObject.transform.position.z));
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
        }
    }
        

    /// <summary>
    /// Запускает EndTurn() для сбитой кегли, останавливает предыдущую корутину если она была запущена
    /// </summary>
    /// <param name="pin">Сбитая кегля</param>
    public void PinHit(Pin pin)
    {
        if (!_endTurn)
        {
            mainCamera.target = mainCamera.cameraPoints[0].transform;
        
            if (_endTurnCorutine != null)
            {
                StopCoroutine(_endTurnCorutine);
            }
            _endTurnCorutine = StartCoroutine(EndTurn());
            if (pin != null)
            {
                HitPins.Add(pin); 
            }
        }

    }

    /// <summary>
    ///  
    /// </summary>
    IEnumerator EndTurn()
    {   
        yield return new WaitForSeconds(2);
        // определяем первый или второй бросок
        bool firstThrow = players[CurrentPlayerIndex].CurrentRound % 2 == 0 &&
                          players[CurrentPlayerIndex].CurrentRound < 18;
        _endTurn = true;
        
        // поднимаем кегли если это был первый бросок и сбиты не все кегли
        if (firstThrow && HitPins.Count != 10)
        {
            pins.LiftUp();
            yield return  new WaitForSeconds(1);
        }
        
        // сборщик кеглей
        _pinsCleanerAnimator.Play("Clean");
        yield return new WaitForSeconds(_pinsCleanerAnimationClip.length);
        
        players[CurrentPlayerIndex].AddScore(HitPins.Count);
        players[CurrentPlayerIndex].CurrentRound++; 

        // добавляем еще +1 к счету раундов если при первом броске сбиты все кегли
        if (firstThrow && HitPins.Count == 10)
        {
            players[CurrentPlayerIndex].CurrentRound++; 
        }
        
        // опускаем кегли если это первый бросок
        if (firstThrow && HitPins.Count != 10)
        {
            pins.LiftDown();
            RestartBall();
        }
        else
        {
            // меняем игрока на следующего если это второй бросок
            if (CurrentPlayerIndex+1 < players.Count)
            {
                CurrentPlayerIndex++;
            }
            else
            {
                CurrentPlayerIndex = 0;
            }
            RestartBall();
            RestartPins();
        }
        
        yield return  new WaitForSeconds(1);
        pins.Collide(true);
        
        Debug.Log("Ход: "+ players[CurrentPlayerIndex].Name);

        _endTurn = false;
    }
    
}

