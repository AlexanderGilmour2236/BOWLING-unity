using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public Ball ball;

    public Pins pins;
    public List<Pin> HitPins;
    
    private bool _ballThrown;
    
    private Coroutine _endTurnCorutine;

    public bool EndTurn { get; private set; }

    private Vector3 _ballDirection;
    private bool _mousePressed;
    private Vector3 _mouseWorldPosition;
    private Vector2 _mouseDownPosition;
    private Vector2 _mouseUpPosition;

    private DateTime _mouseDownTime;
    private DateTime _mouseUpTime;
    
    public float maxSlideTime = 500;

    public CameraBehaviour mainCamera;
    
    [SerializeField]
    private Animator pinsCleanerAnimator;
    [SerializeField] 
    private AnimationClip pinsCleanerAnimationClip;

    public List<Player> players;
    private int _currentPlayerIndex;
    private void Start()
    {
        HitPins = new List<Pin>();
    }

    public void GameStart()
    {
        if (players.Count < 1)
        {
            return;
        }
        _currentPlayerIndex = 0;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("SampleScene");
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
        if (!EndTurn)
        {
            
            // при нажатии левой кнопки мыши запоминаются координаты и время нажатия
            if (Input.GetMouseButtonDown(0))
            {
                if (!_ballThrown)
                {
                    _mouseDownTime = DateTime.Now;
                    _mouseDownPosition = Input.mousePosition;
                    _mousePressed = true;
                }
    
            }
            
            // при зажатой левой кнопке миши шар выполняет поворот относительно курсора
            if (Input.GetMouseButton(0))
            {

                if (_mousePressed)
                {
                    if (!_ballThrown)
                    {
                        _mouseWorldPosition = mainCamera.Camera.ScreenToWorldPoint(
                            Input.mousePosition + new Vector3(0, 0, pins.gameObject.transform.position.z));
                        ball.transform.LookAt(new Vector3(
                            Mathf.Clamp(_mouseWorldPosition.x, -ball.AbsMaxRotation, ball.AbsMaxRotation), 0, 0));
                    }
                }

                
                if (ball.MouseDown)
                {
                    
                    if (!_ballThrown)
                    {
                        // движение шара до броска
                        _mouseWorldPosition = mainCamera.Camera.ScreenToWorldPoint(
                            Input.mousePosition + new Vector3(0, 0, 1));
                        
                        _mouseWorldPosition.x = Mathf.Clamp(_mouseWorldPosition.x, -0.5f, 0.5f);
                        _mouseWorldPosition.y = ball.transform.position.y;
                        _mouseWorldPosition.z = ball.transform.position.z;
                        
                        ball.MoveTo(_mouseWorldPosition);
                    }
                    else
                    {
                        // движение шара после броска
                        _mouseWorldPosition = mainCamera.Camera.ScreenToWorldPoint(
                            Input.mousePosition + new Vector3(0, 0, 1));
                        
                        //_mouseWorldPosition.x = Mathf.Clamp(ball.transform.position.x, -0.5f, 0.5f);
                        _mouseWorldPosition.x = _mouseWorldPosition.x - ball.transform.position.x;
                        _mouseWorldPosition.y = ball.transform.position.y;
                        _mouseWorldPosition.z = ball.transform.position.z;
                        
                        ball.MoveTo(_mouseWorldPosition);
                    }
                }

                
            }
            
            // при отжатии левой кнопки мыши высчитывается сила броска и выполняется функция ball.throw()
            if (Input.GetMouseButtonUp(0) && _mousePressed)
            {
                if (!_ballThrown)
                {
                    _mouseUpTime = DateTime.Now;
                    _mouseUpPosition = Input.mousePosition;
                
                    if(_mouseUpPosition.y > _mouseDownPosition.y && 
                       (_mouseUpPosition-_mouseDownPosition).magnitude > Screen.height/5 &&
                       (_mouseUpPosition-_mouseDownPosition).y > Screen.height/5
                    )
                    {
                        if (_ballThrown == false)
                        {
                            _ballThrown = true;

                            //Screen.height = 100%
                            //SlideLength = x%

                            float screenPercent = (_mouseUpPosition.y - _mouseDownPosition.y) * 100 / Screen.height;
                        
                            // maxBallSpeed = 100%
                            // x = screenPercent

                            float strength = screenPercent * ball.maxStrength / 100;
                        
                            float deltaTime = (float)(_mouseUpTime - _mouseDownTime).TotalSeconds;

                            if (deltaTime >= maxSlideTime)
                            {
                                strength = ball.minStrength;
                            }
                            else
                            {
                                // deltatime от 0 до maxSlideTime-1 делится на максимальное время свайпа для броска
                                // получая величину от ~0 до 1, на которую делится сила броска
                                float timeEffect =
                                    deltaTime / maxSlideTime;

                                strength /= timeEffect;
                            }
                        
                            ball.Strength = strength;
                            ball.Throw();
                        }
                    }
                    _mousePressed = false;  
                }
            }
        }
        
        
        
    }
        

    /// <summary>
    /// Запускает EndTurn() для сбитой кегли, останавливает предыдущую корутину если она была запущена
    /// </summary>
    /// <param name="pin">Сбитая кегля</param>
    public void PinHit(Pin pin)
    {
        if (!EndTurn)
        {
            mainCamera.target = mainCamera.cameraPoints[0].transform;
            
            pinsCleanerAnimator.Play("SetCleaner");
            if (_endTurnCorutine != null)
            {
                StopCoroutine(_endTurnCorutine);
            }
            _endTurnCorutine = StartCoroutine(EndTurnCorutine());
            if (pin != null)
            {
                HitPins.Add(pin); 
            }
        }

    }

    /// <summary>
    ///  
    /// </summary>
    IEnumerator EndTurnCorutine()
    {   
        yield return new WaitForSeconds(1);
        // определяем первый или второй бросок
        bool firstThrow = players[_currentPlayerIndex].CurrentRound % 2 == 0 &&
                          players[_currentPlayerIndex].CurrentRound < 18;
        EndTurn = true;
        
        // поднимаем кегли если это был первый бросок и сбиты не все кегли
        if (firstThrow && HitPins.Count != 10)
        {
            pins.LiftUp();
        }
        
        // сборщик кеглей
        pinsCleanerAnimator.Play("Clean");
        yield return new WaitForSeconds(pinsCleanerAnimationClip.length);
        
        players[_currentPlayerIndex].AddScore(HitPins.Count);
        players[_currentPlayerIndex].CurrentRound++; 

        // добавляем еще +1 к счету раундов если при первом броске сбиты все кегли
        if (firstThrow && HitPins.Count == 10)
        {
            players[_currentPlayerIndex].CurrentRound++; 
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
            if (_currentPlayerIndex+1 < players.Count)
            {
                _currentPlayerIndex++;
            }
            else
            {
                _currentPlayerIndex = 0;
            }
            RestartBall();
            RestartPins();
        }
        
        yield return  new WaitForSeconds(1);
        pins.Collide(true);
        
        Debug.Log("Ход: "+ players[_currentPlayerIndex].Name);

        EndTurn = false;
    }
    
}

