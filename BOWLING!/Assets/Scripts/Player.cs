using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int id;
    public string name;
    public int pinshit;
    public int highscore;
    
    public int CurrentFrameIndex { get; private set; }
    public Frame CurrentFrame => Frames[CurrentFrameIndex];
    /// <summary>
    /// Массив представляющий общее количество очков за игру на каждом фрейме
    /// </summary>
    public int[] FrameTotalScore { get; private set; }
    /// <summary>
    /// Общее количество очков за игру
    /// </summary>
    public int TotalScore { get; private set;}
    public bool IsGameOver => CurrentFrameIndex >= Frames.Count;
    
    public List<Frame> Frames { get; private set; }
    
    public Player()
    {
        Restart();
    }

    /// <summary>
    /// Представляет десятый фрейм в виде трёх чисел
    /// </summary>
    public int[] TenthFrame
    {
        get
        {
            int[] score = new int[] {-1, -1, -1};
            score[0] = Frames[9].FirstThrowScore;
        
            if (Frames.Count >= 10)
            {
                if (!Frames[9].IsStrike)
                {
                    score[1] = Frames[9].SecondThrowScore;   
                }
            }
            if (Frames.Count >= 11)
            {
                if (Frames[9].IsStrike)
                {
                    score[1] = Frames[10].FirstThrowScore;
                    if (!Frames[10].IsStrike)
                    {
                        score[2] = Frames[10].SecondThrowScore;  
                    }
                }
                else
                {
                    score[2] = Frames[10].FirstThrowScore;
                }
            }
            if (Frames.Count == 12)
            {
                score[2] = Frames[11].FirstThrowScore;
            }

            return score;
        }
    }
    public void Restart()
    {
        pinshit = 0;
        highscore = 0;
        TotalScore = 0;
        FrameTotalScore = new int[10];
        for (int i = 0; i < 10; i++)
        {
            FrameTotalScore[i] = -1;
        }
        Frames = new List<Frame>();
        for (int i = 0; i < 10; i++)
        {
            Frames.Add(new Frame());  
        }

        CurrentFrameIndex = 0;
    }
    
    public string ScoreString()
    {
        StringBuilder str = new StringBuilder();
        for(int i = 0; i< Frames.Count; i++)
        {
            str.Append(i+1 + ")" + Frames[i].FirstThrowScore.ToString() + "/" + Frames[i].SecondThrowScore.ToString() + " | ");
        }

        return str.ToString();
    }

    private void ScoreCount()
    {
        // Подсчет общего количества очков для каждого фрейма
        for (int i = 0; i < FrameTotalScore.Length; i++)
        {
            if (FrameTotalScore[i] == -1 && Frames[i].IsComplete)
            {
                // Если был забит страйк
                if (Frames[i].IsStrike)
                {
                    if (i < 8)
                    {
                        if (Frames[i + 1].IsComplete)
                        {
                            if (Frames[i + 1].IsStrike)
                            {
                                if (i == 7)
                                {
                                    if (TenthFrame[0] != -1)
                                    {
                                        FrameTotalScore[i] = TotalScore +=
                                            Frames[i].Total + Frames[i + 1].FirstThrowScore + TenthFrame[0];
                                    }
                                }
                                else
                                {
                                    if (Frames[i + 2].FirstThrowScore != -1)
                                    {
                                        FrameTotalScore[i] = TotalScore += Frames[i].Total + Frames[i+1].FirstThrowScore + Frames[i+2].FirstThrowScore;
                                    }
                                    else
                                    {
                                        if (TenthFrame[0] != -1)
                                        {
                                            FrameTotalScore[i] = TotalScore += Frames[i].Total + Frames[i+1].FirstThrowScore + TenthFrame[0];
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                FrameTotalScore[i] = TotalScore += Frames[i].Total + Frames[i+1].FirstThrowScore + Frames[i+1].SecondThrowScore;
                            }
                        } 
                    }
                    else if (i == 8)
                    {
                        if (TenthFrame[0] != -1 && TenthFrame[1] != -1)
                        {  
                            FrameTotalScore[i] = TotalScore += Frames[i].Total + TenthFrame[0] + TenthFrame[1];
                        }
                    }
                    else
                    {
                        if (TenthFrame[0] != -1 && TenthFrame[1] != -1 && TenthFrame[2] != -1)
                        {
                            FrameTotalScore[i] = TotalScore += TenthFrame[0] + TenthFrame[1] + TenthFrame[2];
                        }
                    }
                }
                // Если был забит spare
                else if (Frames[i].IsSpare)
                {
                    if (i != 9)
                    {
                        if (Frames[i + 1].FirstThrowScore!=-1)
                        {
                            FrameTotalScore[i] = TotalScore += Frames[i].Total + Frames[i+1].FirstThrowScore;
                        }
                    }
                    else
                    {
                        if (TenthFrame[0] != -1 && TenthFrame[1] != -1 && TenthFrame[2] != -1)
                        {
                            FrameTotalScore[i] = TotalScore += TenthFrame[0] + TenthFrame[1] + TenthFrame[2];
                        }
                    }
                } 
                else
                {
                    FrameTotalScore[i] = TotalScore += Frames[i].Total;
                }
            }
        }
    }
    /// <summary>
    /// Добавляет очки в текущий фрейм
    /// </summary>
    /// <param name="score">Количество очков</param>
    public void AddScore(int score)
    {
        CurrentFrame.AddScore(score);
        
        ScoreCount();
        
        // 10-й фрейм (3 броска) реализован через добавление новых фреймов в случае если игрок выбил страйк или spare
        if (CurrentFrameIndex >= 9)
        {
            // если в последнем фрейме был выбит страйк, добавляется еще один
            if (Frames.Count == 10 && Frames[9].IsStrike)
            {
                Frames.Add(new Frame());
            }
            else
            // если в последнем фрейме был выбит spare, добавляется еще один фрейм с одним броском
            if (Frames.Count == 10 && Frames[9].IsSpare)
            {
            
                Frames.Add(new Frame(-1,0));
            }
            else
            // если уже был добавлен фрейм и в последние два броска был выбит страйк, добавляется еще один бросок
            if (Frames.Count == 11 && Frames[9].IsStrike && Frames[10].IsStrike)
            {
                Frames.Add(new Frame(-1,0));
            } 
        }
    }

    /// <summary>
    /// Меняет текущий фрейм на следующий
    /// </summary>
    public void NextFrame()
    {
        CurrentFrameIndex++; 
    }
}
