using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    
    #region Events

    public delegate void onGameStart(object sender, ChosePlayersMenuResultArgs a);
    public static event onGameStart OnGameStart;

    public delegate void onGamePaused(bool showParameter);
    public static event onGamePaused OnGamePaused;

    public delegate void onMainMenu();
    public static event onMainMenu OnMainMenu;

    public delegate void onCameraChanged(Transform cameraPosition);
    public static event onCameraChanged OnCameraChanged;
    
    #endregion
    
    public Menu CurrentMenu { get; private set; }
    
    [SerializeField] private GameObject backgroundPanel;
    
    [SerializeField] private Menu mainMenu;
    [SerializeField] private PlayerChoseMenu playerChoseMenu;
    [SerializeField] private SliderMenu playerCountMenu;
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private StatisticsMenu statisticsMenu;
    [SerializeField] private CustomizeMenu customizeMenu;
    
    [SerializeField] private Button backButton;
    [SerializeField] private Button pauseButton;
    
    private void Start()
    {
        playerChoseMenu.OnMenuResult += OnStartGame;
        
        playerCountMenu.OnOkButtonClick.AddListener(() => SelectPlayerNamesMenu(Convert.ToInt32((playerCountMenu.SliderValue))));
    }

    private void OnDestroy()
    {
        OnGameStart = delegate(object sender, ChosePlayersMenuResultArgs args) {  };
        OnGamePaused = delegate(bool parameter) {  };
        OnMainMenu = delegate {  };
    }

    public void MainMenu()
    {     
        if (OnMainMenu != null)
        {
            OnMainMenu();
        }
        HideAllMenues();
        
        NextMenu(mainMenu);
    }

    public void SelectStatisticsMenu()
    {
        NextMenu(statisticsMenu);
    }
    
    private void OnStartGame(object sender, ChosePlayersMenuResultArgs a)
    {
        pauseButton.gameObject.SetActive(true);
        HideAllMenues();
        if (OnGameStart != null)
        {
            OnGameStart(sender,a);
        }
    }
    
    public void SelectPlayerCountMenu()
    {
        NextMenu(playerCountMenu);
    }
    
    public void SelectPlayerNamesMenu(int playerCount)
    {
        
        playerChoseMenu.PlayerCount = playerCount;
        NextMenu(playerChoseMenu);
    }

    
    public void PauseMenu(bool showParameter)
    {
        if (OnGamePaused != null)
        {
            OnGamePaused(showParameter);
        }

        if (showParameter)
        {
            pauseButton.gameObject.SetActive(false);
            NextMenu(pauseMenu);
        }
        else
        {
            pauseButton.gameObject.SetActive(true);
            HideAllMenues();
        }

    }

    public void SelectCustomizeBallMenu()
    {
        NextMenu(customizeMenu);
    }
    
    public void NextMenu(Menu menu)
    {
        gameObject.SetActive(true);
        
        if (CurrentMenu != null)
        {
            CurrentMenu.Show(false);
            menu.prevMenu = CurrentMenu; 
        }
        CurrentMenu = menu;
        CurrentMenu.Show(true);

        if (CurrentMenu.cameraPosition != null)
        {
            
        }
        
        backButton.gameObject.SetActive((CurrentMenu.prevMenu!=null));
    }
    
    public void Back()
    {
        if (CurrentMenu.prevMenu!=null)
        {
            CurrentMenu.Show(false);

            CurrentMenu = CurrentMenu.prevMenu;
            
            CurrentMenu.Show(true);

            backButton.gameObject.SetActive((CurrentMenu.prevMenu!=null));
        }
    }

    
    public void HideAllMenues()
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.Show(false);
        }
        CurrentMenu = null;
        gameObject.SetActive(false);
    }

    public void HideButtons()
    {
        backButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
    }
}
