using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame
{
    public int FirstThrowScore { get; private set; }
    public int SecondThrowScore { get; private set; }

    /// <summary>
    /// Количество очков за два броска
    /// </summary>
    public int Total
    {
        get
        {
            int total = 0;
            if (FirstThrowScore != -1) total += FirstThrowScore;
            if (SecondThrowScore != -1) total += SecondThrowScore;
            return total;
        }
    }

    public bool IsComplete => FirstThrowScore != -1 && SecondThrowScore != -1;

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
            else
            {
                return -1;
            }
        }
    }
    
    public bool IsStrike { get; private set; }
    public bool IsSpare { get; private set; }
    
    public Frame()
    {
        Restart();
    }
    public Frame(int firstThrowScore, int secondThrowScore)
    {
        FirstThrowScore = firstThrowScore;
        SecondThrowScore = secondThrowScore;
    }
    /// <summary>
    /// Обнуляет фрейм до начальных значений
    /// </summary>
    void Restart()
    {
        FirstThrowScore = -1;
        SecondThrowScore = -1;
        
        IsSpare = false;
        IsStrike = false;
    }

    /// <summary>
    /// Добавляет очки в один из бросков
    /// </summary>
    /// <param name="score"></param>
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
            else if (SecondThrowScore == -1)
            {
                SecondThrowScore = score;
            }
            
            if (FirstThrowScore + SecondThrowScore == 10)
            {
                if (!IsStrike)
                {
                    IsSpare = true;
                }

            }
    }
}
