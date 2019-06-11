using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSheet : MonoBehaviour
{
    public Player CurrentPlayer { get; private set; }

    [SerializeField] Text playerNameLabel;
    [SerializeField] Text TotalScoreLabel;
    private List<UiFrame> UIFrames = new List<UiFrame>();
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
                        (frame.transform.childCount >= 4) ? frame.transform.GetChild(3).GetComponent<Text>() : null
                    )
                );
            }
        }
    }

    public void Show()
    {
        if (_isHidden)
        {
            _isHidden = false;
            _animator.Play("LiftDown");
            
        }
    }

    public void Hide()
    {
        if (!_isHidden)
        {
            _isHidden = true;
            _animator.Play("LiftUp");
        }
    }

    public void LoadPlayer(Player player)
    {
        playerNameLabel.text = player.name;
        TotalScoreLabel.text = player.TotalScore.ToString();
        // Вывод очков до 9 фрейма
        for (int i = 0; i < UIFrames.Count-1; i++)
        {
            // Вывод общего количества очков за игру до текущего фрейма
            if (player.FrameTotalScore[i] != -1)
            {
                UIFrames[i].TotalThrowLabel.text = player.FrameTotalScore[i].ToString();   
            }
            else
            {
                UIFrames[i].TotalThrowLabel.text = "";
            }
            
            UIFrames[i].FirstThrowLabel.text = player.Frames[i].FirstThrowScore.ToString();

            if (player.Frames[i].IsStrike)
            {
                UIFrames[i].FirstThrowLabel.text = "X";
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

        // Десятый фрейм
        int[] tenthFrame = player.TenthFrame;
        
        for (int i = 0; i < tenthFrame.Length; i++)
        {
            if (tenthFrame[i] == -1)
            {
                UIFrames[9][i].text = "";
            }
            else
            if (tenthFrame[i] == 10)
            {
                UIFrames[9][i].text = "X";
                if (i > 0 && tenthFrame[i - 1] == 0)
                {
                    UIFrames[9][i].text = "/";
                }
            }
            else
            {
                UIFrames[9][i].text = tenthFrame[i].ToString();
                if (tenthFrame[0] != 10 && tenthFrame[0] + tenthFrame[1] == 10)
                {
                    UIFrames[9][1].text = "/";
                }
                if (tenthFrame[1] != 10 && tenthFrame[0] == 10 && tenthFrame[1] + tenthFrame[2] == 10)
                {
                    UIFrames[9][2].text = "/";
                }
            }
        }
        
        if (player.FrameTotalScore[9] != -1)
        {
            UIFrames[9].TotalThrowLabel.text = player.FrameTotalScore[9].ToString();
        }
        else
        {
            UIFrames[9].TotalThrowLabel.text = "";
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
    public Text this[int key]
    {
        get
        {
            if (key == 0) return FirstThrowLabel;
            if (key == 1) return SecondThrowLabel;
            if (key == 2) return ThirdThrowLabel;
            if (key == 3) return TotalThrowLabel;
            return null;
        }
    }
    
}