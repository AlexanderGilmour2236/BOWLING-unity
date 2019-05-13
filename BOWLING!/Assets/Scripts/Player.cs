using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public string Name;
    public int CurrentFrameIndex { get; private set; }
    public Frame CurrentFrame => Frames[CurrentFrameIndex];

    public bool IsGameOver => CurrentFrameIndex >= Frames.Count;
    
    public List<Frame> Frames { get; private set; }
    
    void Start()
    {
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
    /// <summary>
    /// Добавляет очки в текущий фрейм
    /// </summary>
    /// <param name="score">Количество очков</param>
    public void AddScore(int score)
    {
        CurrentFrame.AddScore(score);

        // 10-й фрейм (3 броска)
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
