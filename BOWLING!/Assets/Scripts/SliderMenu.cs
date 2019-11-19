using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMenu : Menu
{
    [SerializeField] private Button OkButton;
    public Button.ButtonClickedEvent OnOkButtonClick => OkButton.onClick;

    [SerializeField] private Slider slider;
    [SerializeField] private Text sliderValueLabel;
    
    public float SliderValue => slider.value;
    public int PlayersCountLabel
    {
        set => sliderValueLabel.text = slider.value.ToString();
    }
}
