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
        chosePlayerPanel.SetActive(true);
        statsPanel.SetActive(false);
        errorText.text = "";
    }

    public void ShowStatistics(Player player)
    {
        if (player != null)
        {
           chosePlayerPanel.SetActive(false);
           statsPanel.SetActive(true);
   
           hitPinsText.text = player.pinshit.ToString();
           highScoreText.text = player.highscore.ToString(); 
        }
        else
        {
            errorText.text = "Игрок с таким именем не найден";
        }

    }
    
    void OnOkButton()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            DBController.Instance.GetStats(inputField.text);
        }
    }
}
