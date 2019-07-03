using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsMenu : Menu
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button okButton;
    [SerializeField] private GameObject chosePlayerPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private Image statsProgressBar;
    
    [SerializeField] private Text nameText;
    [SerializeField] private Text hitPinsText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text strikesText;
    [SerializeField] private Text errorText;
    private void Start()
    {
        DBController.OnGetUser += ShowStatistics;
        okButton.onClick.AddListener(OnOkButton);
        
    }

    public override void Show(bool showParameter)
    {
        base.Show(showParameter);
        okButton.gameObject.SetActive(true);
        chosePlayerPanel.SetActive(true);
        statsPanel.SetActive(false);
        ShowError(null);
    }

    public void ShowError(Exception ex)
    {
        if (ex != null)
        {
            errorText.text = ex.HResult.ToString();
        }
        else
        {
            errorText.text = "";
        }
       
    }
    
    public void ShowStatistics(Player player)
    {
        
        if (player != null)
        {
           chosePlayerPanel.SetActive(false);
           statsPanel.SetActive(true);

           nameText.text = "Игрок: " + player.name;
           hitPinsText.text = player.pinshit.ToString();
           highScoreText.text = player.highscore.ToString();
           strikesText.text = player.strikes.ToString();
        }
        else
        {
            errorText.text = "Игрок с таким именем не найден";
            okButton.gameObject.SetActive(true);
        }

    }
    
    void OnOkButton()
    {
        ShowError(null);
        okButton.gameObject.SetActive(false);
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            DBController.Instance.GetStats(inputField.text);
        }
    }
}
