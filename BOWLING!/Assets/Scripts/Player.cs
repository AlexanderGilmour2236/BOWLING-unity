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
    
    private List<Frame> Frames;
    
    void Start()
    {
        Frames = new List<Frame>();
        for (int i = 0; i < 10; i++)
        {
            Frames.Add(new Frame());  
        }

        Frames[9].IsTenthFrame = true;
        CurrentFrameIndex = 0;
    }

    public string ScoreString()
    {
        StringBuilder str = new StringBuilder();
        foreach (var frame in Frames)
        {
            str.Append(frame.FirstThrowScore.ToString() + "/" + frame.SecondThrowScore.ToString() + " | ");
        }

        return str.ToString();
    }
    public void AddScore(int score)
    {
        CurrentFrame.AddScore(score);
        if (CurrentFrame.IsComplete)
        {
            CurrentFrameIndex++;
        }

    }
}
