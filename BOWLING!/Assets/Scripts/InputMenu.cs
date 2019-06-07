using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMenu : Menu
{
    [SerializeField] private Button okButton;
    [SerializeField] private Slider slider;
    [SerializeField] private Text sliderValueLabel;
    
    public int PlayersCountLabel
    {
        set => sliderValueLabel.text = slider.value.ToString();
    }
    private void Start()
    {
        okButton.onClick.AddListener(() => MenuController.Instance.SelectPlayerNamesMenu(Convert.ToInt32((slider.value))));
    }
}
