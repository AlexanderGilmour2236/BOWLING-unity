using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSheet : MonoBehaviour
{
    public Player CurrentPlayer { get; private set; }

    public Text playerNameLabel;
    public Text TotalScoreLabel;
    public List<UiFrame> UIFrames = new List<UiFrame>();
    private bool _isHidden = true;

private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        foreach (Transform frame in transform)
        {
            if (frame.gameObject.tag == "frame")
            {
                
                UIFrames.Add(
                    new UiFrame(
                    frame.transform.GetChild(0).GetComponent<Text>(),
                    frame.transform.GetChild(1).GetComponent<Text>(),
                    frame.transform.GetChild(2).GetComponent<Text>(),
                    //10-й фрейм содержит третье поле
                    (frame.transform.childCount>=4) ? frame.transform.GetChild(2).GetComponent<Text>() : null
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
        
        // Расчет очков с 1 до первых двух бросков 10 фрейма
        for (int i = 0; i < UIFrames.Count; i++)
        {
            UIFrames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();
            
            if (player.Frames[i].IsStrike)
            {   UIFrames[i].FirstThrowLabel.text = "X";
                if (i < 9)
                {
                    UIFrames[i].SecondThrowLabel.text = "-";
                }
                continue;
            }

            if (player.Frames[i].IsSpare)
            {
                UIFrames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();
                UIFrames[i].SecondThrowLabel.text = "/";
                continue;
            }
            
            if (player.Frames[i].FirstThrowScore == -1)
            {
                UIFrames[i].FirstThrowLabel.text = "";
            }
            else
            {
                UIFrames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();
            }
            
            if (player.Frames[i].SecondThrowScore == -1)
            {
                UIFrames[i].SecondThrowLabel.text = "";
            }
            else
            {
                UIFrames[i].SecondThrowLabel.text = player.Frames[i].SecondThrowScore.ToString();
            }
        }

        if (player.Frames.Count == 10)
        {
            UIFrames[9].ThirdThrowLabel.text = "";
        }
        // Если в 10-м фрейме был выбит страйк или spare
        if (player.Frames.Count == 11)
        {
            if (player.Frames[9].IsSpare)
            {
                // Если в 10 фрейме за первые два броска был выбит spare
                if (player.Frames[10].FirstThrowScore != -1)
                {
                    if (player.Frames[10].IsStrike)
                    {
                        UIFrames[9].ThirdThrowLabel.text = "X";
                    }
                    else
                    {
                        UIFrames[9].ThirdThrowLabel.text = player.Frames[10].FirstThrowScore.ToString();    
                    }
                }
                else
                {
                    UIFrames[9].ThirdThrowLabel.text = "";
                }
            }
            else
            {
                // Если первый бросок был Страйк
                if (player.Frames[10].FirstThrowScore != -1)
                {
                    UIFrames[9].SecondThrowLabel.text = player.Frames[10].FirstThrowScore.ToString();   
                }
                else
                {
                    UIFrames[9].SecondThrowLabel.text = "";
                }

                if (player.Frames[10].SecondThrowScore != -1)
                {
                    if (player.Frames[10].IsSpare)
                    {
                        UIFrames[9].ThirdThrowLabel.text = "/";
                    }
                    else
                    {
                        UIFrames[9].ThirdThrowLabel.text = player.Frames[10].SecondThrowScore.ToString(); 
                    }
                }
                else
                {
                    UIFrames[9].ThirdThrowLabel.text = "";
                }
            }
        }
        
        // Если в 10 фрейме было выбито 2 страйка 
        if (player.Frames.Count == 12)
        {
            UIFrames[9].FirstThrowLabel.text = "X";
            UIFrames[9].SecondThrowLabel.text = "X";
            if (player.Frames[11].IsStrike)
            {
                UIFrames[9].ThirdThrowLabel.text = (player.Frames[11].IsStrike) ? "X" : player.Frames[11].FirstThrowScore.ToString();
            }
            else
            {
                if (player.Frames[11].FirstThrowScore != -1)
                {
                    UIFrames[9].ThirdThrowLabel.text = player.Frames[11].FirstThrowScore.ToString();
                }
                else
                {
                    UIFrames[9].ThirdThrowLabel.text = "";
                }
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