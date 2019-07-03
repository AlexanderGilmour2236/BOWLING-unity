using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public enum GameMode{
   MainMenu,
   Game,
   Options,
   HighScores,
   GameOver,
   Pause
}

public class GameController : MonoBehaviour
{
    #region Singleton

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject ins = new GameObject("GameController");
                ins.AddComponent<GameController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion
    
    public GameMode GameMode { get; private set; }
    
    [SerializeField]
    private Ball ball;
    [SerializeField]
    private Pins pins;
    
    /// <summary>
    /// Коллекция сбитых кеглей
    /// </summary>
    private List<Pin> _hitPins;
    
    /// <summary>
    /// true если шар брошен
    /// </summary>
    private bool _ballThrown;

    private Coroutine _endTurnCoroutine;

    /// <summary>
    /// true если это конец хода (подсчет очков)
    /// </summary>
    public bool EndTurn { get; private set; }
    
    private bool _mousePressed;
    private Vector3 _mouseWorldPosition;
    private Vector2 _mouseDownPosition;
    private Vector2 _mouseUpPosition;

    private DateTime _mouseDownTime;
    private DateTime _mouseUpTime;

    [SerializeField] private MenuController menuController;
    
    
    /// <summary>
    /// Время слайда, при достижении которого шар будет лететь с минимальной скоростью
    /// </summary>
    [SerializeField]
    private float maxSlideTime = 500;

    [SerializeField]
    private CameraBehaviour mainCamera;

    [SerializeField] private ScoreSheet scoreSheet;
    /// <summary>
    /// Всплывающий текст после особых ходов (страйк, спэйр)
    /// </summary>
    [SerializeField] private PopUpLabel popUpLabel;

    [SerializeField] private GameObject pinsCleaner;
    private Animator _pinsCleanerAnimator;
    /// <summary>
    /// Клип для поднятия граблей
    /// </summary>
    [SerializeField] private AnimationClip pinsCleanAnimationClip;

    public List<Player> Players { get; private set; }
    public int CurrentPlayerIndex { get; private set; }
    public Player CurrentPlayer => Players[CurrentPlayerIndex];

    private void Start()
    {
        MenuController.OnGameStart += GameStart;
        MenuController.OnGamePaused += GamePause;
        MenuController.OnMainMenu += ToMainMenu;
        
        _hitPins = new List<Pin>();
        _pinsCleanerAnimator = pinsCleaner.GetComponent<Animator>();
        
        menuController.MainMenu();
    }
    
    private void ToMainMenu()
    {
        pins.LiftDown();
        scoreSheet.Hide();
        mainCamera.target = mainCamera.cameraPoints[0].transform;
        if (GameMode == GameMode.Pause)
        {
            GamePause(false);
            _pinsCleanerAnimator.Play("Clean");
        }
        
        GameMode = GameMode.MainMenu;
        if (_endTurnCoroutine != null)
        {
            StopCoroutine(_endTurnCoroutine);  
        }
    }

    public void GameStart(object sender, ChosePlayersMenuResultArgs a)
    {
        if (a.Players == null || a.Players.Count < 1)
        {
            return;
        }
        if (_endTurnCoroutine != null)
        {
            StopCoroutine(_endTurnCoroutine);  
        }

        _mousePressed = false;
        EndTurn = false;
        _hitPins.Clear();
        
        CurrentPlayerIndex = 0;
        
        GameMode = GameMode.Game;
        
        RestartBall();
        RestartPins();
        
        Players = a.Players;
    }

    private void GameOver()
    {
        Player win = new Player();
        int max = 0;
        foreach (Player player in Players)
        {
            if (player.TotalScore > max)
            {
                max = player.TotalScore;
                win = player;
            }
        }

        if (max != 0)
        {
            CurrentPlayerIndex = Players.IndexOf(win);
        }

        foreach (Player player in Players)
        {
            player.highscore = player.TotalScore;
            int totalPinsHit = 0;
            int strikes = 0;
            foreach (Frame frame in player.Frames)
            {
                if (frame.IsStrike) strikes++;
                totalPinsHit += frame.Total;
            }
            player.pinshit = totalPinsHit;
            player.strikes = strikes;
        }
        DBController.Instance.UpdatePlayers(Players);
        
        CurrentPlayer.name += " win!";
        scoreSheet.LoadPlayer(CurrentPlayer);
        scoreSheet.Show();

        GameMode = GameMode.GameOver;
    }
    
    public void GamePause(bool showParameter)
    {
        if (showParameter)
        {
            GameMode = GameMode.Pause;
            Time.timeScale = 0; 
        }
        
        if (!showParameter)
        {
            GameMode = GameMode.Game;
            Time.timeScale = 1;
        } 
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void Strike()
    {
        pins.Strike();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// Возвращает кегли в начальные позиции
    /// </summary>
    private void RestartPins()
    {
        pins.Restart();
        _hitPins.Clear();
    }

    /// <summary>
    /// Возвращает шар в начальные позиции
    /// </summary>
    private void RestartBall()
    {
        _ballThrown = false;
        ball.ToStart();
        mainCamera.target = ball.transform;
    }

    private void Update()
    {
        if (GameMode == GameMode.Game)
        {
             if (Input.GetMouseButtonDown(0))
            {
                scoreSheet.Hide();
            }
    
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
        else if(GameMode == GameMode.GameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                menuController.MainMenu();
            }
        }
       
    }


    /// <summary>
    /// Запускает EndTurn() для сбитой кегли, останавливает предыдущую корутину если она была запущена
    /// </summary>
    /// <param name="pin">Сбитая кегля</param>
    public void PinHit(Pin pin)
    {
        if (!EndTurn && GameMode == GameMode.Game)
        {
            mainCamera.target = mainCamera.cameraPoints[0].transform;

            _pinsCleanerAnimator.Play("SetCleaner");
            if (_endTurnCoroutine != null)
            {
                StopCoroutine(_endTurnCoroutine);
            }

            _endTurnCoroutine = StartCoroutine(EndTurnCorutine());
            if (pin != null && !_hitPins.Contains(pin))
            {
                _hitPins.Add(pin);
            }
        }

    }

    /// <summary>
    /// Меняет текущего игрока на следующего, если игрок последний - меняет текущего игрока на первого
    /// </summary>
    private void NextPlayer()
    {
        if (CurrentPlayerIndex + 1 < Players.Count)
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
        EndTurn = true;

        // определяем первый или второй бросок
        int currentThrow = CurrentPlayer.CurrentFrame.CurrentThrow;
        
        if (currentThrow == 0)
        {
            CurrentPlayer.AddScore(_hitPins.Count);
            if (CurrentPlayer.CurrentFrame.FirstThrowScore == 0)
            {
                popUpLabel.Text = "GUTTER";
                popUpLabel.Show();
            }
        }
        else if (currentThrow == 1)
        {
            CurrentPlayer.AddScore(_hitPins.Count - CurrentPlayer.CurrentFrame.FirstThrowScore);
            if (CurrentPlayer.CurrentFrame.SecondThrowScore == 0)
            {
                popUpLabel.Text = "GUTTER";
                popUpLabel.Show();
            }
        }
        
        
        
        scoreSheet.LoadPlayer(CurrentPlayer);
        scoreSheet.Show();
        
        // поднимаем кегли если это был первый бросок и сбиты не все кегли
        if (!CurrentPlayer.CurrentFrame.IsComplete)
        {
            pins.LiftUp();
        }
        else
        {
            if (CurrentPlayer.CurrentFrame.IsStrike)
            {
                popUpLabel.Text = "STRIKE!";
                popUpLabel.Show();
            }

            if (CurrentPlayer.CurrentFrame.IsSpare)
            {
                popUpLabel.Text = "SPARE";
                popUpLabel.Show();
            }
        }

        // сборщик кеглей
        _pinsCleanerAnimator.Play("Clean");
        yield return new WaitForSeconds(pinsCleanAnimationClip.length);
        
        // опускаем кегли и возвращаем шар в начальную позвицию если это первый бросок
        if (!CurrentPlayer.CurrentFrame.IsComplete)
        {
            pins.LiftDown();
            RestartBall();
        }
        else
        {
            // меняем фрейм на следующий и игрока на следующего если этот фрейм окончен
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
            
            if (!CurrentPlayer.IsGameOver)
            {
                RestartBall();
                RestartPins();
            }
            else
            {
                GameOver();
            }
        }

        yield return new WaitForSeconds(1);
        
        pins.Collide(true);
        EndTurn = false;
    }

}

