using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSheet : MonoBehaviour
{
    public Player CurrentPlayer { get; private set; }

    public Text playerNameLabel;
    public Text TotalScoreLabel;
    public List<UiFrame> Frames = new List<UiFrame>();
    private bool _isHidden = true;

private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        foreach (Transform frame in transform)
        {
            if (frame.gameObject.tag == "frame")
            {
                
                Frames.Add(
                    new UiFrame(
                    frame.transform.GetChild(0).GetComponent<Text>(),
                    frame.transform.GetChild(1).GetComponent<Text>(),
                    frame.transform.GetChild(2).GetComponent<Text>(),
                    //10-й фрейм содержит третье поле
                    (frame.transform.childCount==4) ? frame.transform.GetChild(3).GetComponent<Text>() : null
                    )
                );
            }
        }
    }

    public void Show()
    {
        if (_isHidden)
        {
            _animator.Play("LiftDown");
            _isHidden = false;
        }
    }

    public void Hide()
    {
        if (!_isHidden)
        {
            _animator.Play("LiftUp");
            _isHidden = true;
        }
    }
    
    public void LoadPlayer(Player player)
    {
        playerNameLabel.text = player.Name;
        
        for (int i = 0; i < Frames.Count-1; i++)
        {

            Frames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();
            if (player.Frames[i].IsStrike)
            {   Frames[i].FirstThrowLabel.text = "X";
                Frames[i].SecondThrowLabel.text = "-";
                continue;
            }

            if (player.Frames[i].IsSpare)
            {
                Frames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();
                Frames[i].SecondThrowLabel.text = "/";
                continue;
            }
            
            if (player.Frames[i].FirstThrowScore == -1)
            {
                Frames[i].FirstThrowLabel.text = "";
            }
            else
            {
                Frames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();
            }
            
            if (player.Frames[i].SecondThrowScore == -1)
            {
                Frames[i].SecondThrowLabel.text = "";
            }
            else
            {
                Frames[i].SecondThrowLabel.text = player.Frames[i].SecondThrowScore.ToString();
            }
            
            

        }
    }
}

public class UiFrame
{
    public Text FirstThrowLabel;
    public Text SecondThrowLabel;
    public Text ThirdThrowLabel;
    public Text TotalThrowLabel;

    public UiFrame(Text firstThrow, Text secondThrow, Text total, Text thirdThrow = null)
    {
        FirstThrowLabel = firstThrow;
        SecondThrowLabel = secondThrow;
        TotalThrowLabel = total;
        if (thirdThrow != null) ThirdThrowLabel = thirdThrow;
    }
}