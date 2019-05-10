using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame
{
    public int FirstThrowScore { get; private set; }
    public int SecondThrowScore { get; private set; }
    public int ThirdThrowScore { get; private set; }
    
    public int Total
    {
        get
        {
            if (!IsTenthFrame)
            {
                return FirstThrowScore + SecondThrowScore;
            }
            else
            {
                return FirstThrowScore + SecondThrowScore + ThirdThrowScore; 
            }
        }
    } 

    public bool IsComplete
    {
        get
        {
            if (!IsTenthFrame)
            {
                return FirstThrowScore != -1 && SecondThrowScore != -1;
            }
            else
            {
                return FirstThrowScore != -1 && SecondThrowScore != -1 && ThirdThrowScore != -1;
            }
        }
    }

    public int CurrentThrow
    {
        get
        {
            if (FirstThrowScore == -1)
            {
                return 0;
            }
            else if (SecondThrowScore == -1)
            {
                return 1;
            }
            else if (IsTenthFrame && ThirdThrowScore == -1)
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
    }
    public bool IsTenthFrame { get; set; }
    
    public bool IsStrike { get; private set; }
    public bool IsSpare { get; private set; }
    
    public Frame()
    {
        Restart();
    }
    
    public void Restart()
    {
        FirstThrowScore = -1;
        SecondThrowScore = -1;
        ThirdThrowScore = -1;
        
        IsSpare = false;
        IsStrike = false;
        IsTenthFrame = false;
    }

    public void AddScore(int score)

    {
        if (FirstThrowScore == -1)
        {
            FirstThrowScore = score;
         
            if (score == 10)
            {
                SecondThrowScore = 0;
                IsStrike = true;
            }
        }
        else
        {
            if (SecondThrowScore == -1)
            {
                SecondThrowScore = score;
            }
                
            if (FirstThrowScore + SecondThrowScore == 10)
            {
                IsSpare = true;
            }
        }
        

    }
}
