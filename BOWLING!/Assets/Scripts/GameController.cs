using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Text ScoreLabel;
    public Text PlayerNameLabel;

    public ScoreSheet scoreSheet;
    
    [SerializeField] private Animator pinsCleanerAnimator;
    [SerializeField] private AnimationClip pinsCleanerAnimationClip;

    public List<Player> players;
    public int CurrentPlayerIndex { get; private set; }
    public Player CurrentPlayer => players[CurrentPlayerIndex];

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

        CurrentPlayerIndex = 0;
    }

    public void Strike()
    {
        pins.Strike();
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
        PlayerNameLabel.text = CurrentPlayer.Name;
        ScoreLabel.text = CurrentPlayer.ScoreString();
        
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
                scoreSheet.Hide();
                
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
                }

                if (_ballThrown)
                {
                    // поворот шара после броска
                    _mouseWorldPosition = mainCamera.Camera.ScreenToWorldPoint(
                        Input.mousePosition + new Vector3(0, 0, 1));

                    _mouseWorldPosition.x = _mouseWorldPosition.x - ball.transform.position.x;
                    _mouseWorldPosition.y = ball.transform.position.y;
                    _mouseWorldPosition.z = ball.transform.position.z;

                    ball.MoveTo(_mouseWorldPosition);
                }


            }

            // при отжатии левой кнопки мыши высчитывается сила броска и выполняется функция ball.throw()
            if (Input.GetMouseButtonUp(0) && _mousePressed)
            {
                if (!_ballThrown)
                {
                    _mouseUpTime = DateTime.Now;
                    _mouseUpPosition = Input.mousePosition;

                    if (_mouseUpPosition.y > _mouseDownPosition.y &&
                        (_mouseUpPosition - _mouseDownPosition).magnitude > Screen.height / 5 &&
                        (_mouseUpPosition - _mouseDownPosition).y > Screen.height / 5
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

                            float deltaTime = (float) (_mouseUpTime - _mouseDownTime).TotalSeconds;

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
            if (pin != null && !HitPins.Contains(pin))
            {
                HitPins.Add(pin);
            }
        }

    }

    void NextPlayer()
    {
        if (CurrentPlayerIndex + 1 < players.Count)
        {
            CurrentPlayerIndex++;
        }
        else
        {
            CurrentPlayerIndex = 0;
        }
    }
    
    /// <summary>
    /// Всё что происходит после сбития кегли
    /// </summary>
    IEnumerator EndTurnCorutine()
    {
        yield return new WaitForSeconds(1);

        // определяем первый или второй бросок
        int currentThrow = CurrentPlayer.CurrentFrame.CurrentThrow;

        EndTurn = true;
        
        if (currentThrow == 0)
        {
            CurrentPlayer.AddScore(HitPins.Count);
        }
        else if (currentThrow == 1)
        {
            CurrentPlayer.AddScore(HitPins.Count - CurrentPlayer.CurrentFrame.FirstThrowScore);
        }
        
        scoreSheet.LoadPlayer(CurrentPlayer);
        scoreSheet.Show();
        
        // поднимаем кегли если это был первый бросок и сбиты не все кегли
        if (!CurrentPlayer.CurrentFrame.IsComplete)
        {
            pins.LiftUp();
        }

        // сборщик кеглей
        pinsCleanerAnimator.Play("Clean");
        yield return new WaitForSeconds(pinsCleanerAnimationClip.length);
        
        // опускаем кегли и возвращаем шар в начальную позвицию если это первый бросок
        if (!CurrentPlayer.CurrentFrame.IsComplete)
        {
            pins.LiftDown();
            RestartBall();
        }
        else
        {
            // меняем фрейм на следующий и игрока на следующего если этот фрейм окончен
            if (CurrentPlayer.CurrentFrame.IsComplete)
            {
                CurrentPlayer.NextFrame();

                
                if (CurrentPlayer.CurrentFrameIndex <= 9)
                {
                    NextPlayer();
                }
                else
                {
                    if (CurrentPlayer.IsGameOver)
                    {
                        NextPlayer();
                    }
                }
                
                scoreSheet.LoadPlayer(CurrentPlayer);
                scoreSheet.Show();
                
            }
            
            if (!CurrentPlayer.IsGameOver)
            {
                RestartBall();
                RestartPins();
            }
        }

        yield return new WaitForSeconds(1);
        
        pins.Collide(true);
        
        EndTurn = false;
        yield return new WaitForSeconds(2);
        scoreSheet.Hide();
    }

}

