using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    
    #region Singleton

    private static MenuController _instance;

    public static MenuController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject ins = new GameObject("MenuController");
                ins.AddComponent<MenuController>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    #endregion
    
    public Menu CurrentMenu { get; private set; }
    [SerializeField] private GameObject backgroundPanel;
    
    [SerializeField] private Menu mainMenu;
    [SerializeField] private PlayerChoseMenu playerChoseMenu;
    [SerializeField] private InputMenu playerCountMenu;

    public void MainMenu()
    {
        NextMenu(mainMenu);
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

    public void NextMenu(Menu menu)
    {
        backgroundPanel.SetActive(true);
        
        if (CurrentMenu != null)
        {
            CurrentMenu.Show(false);
            menu.prevMenu = CurrentMenu; 
        }
        CurrentMenu = menu;
        CurrentMenu.Show(true);
    }
    
    public void Back()
    {
        if (CurrentMenu.prevMenu!=null)
        {
            CurrentMenu.Show(false);

            CurrentMenu = CurrentMenu.prevMenu;
            
            CurrentMenu.Show(true);
        }
    }

    public void HideAllMenues()
    {
        mainMenu.gameObject.SetActive(false);
        playerChoseMenu.gameObject.SetActive(false);
        playerCountMenu.gameObject.SetActive(false);
        
        backgroundPanel.SetActive(false);
    }
}
