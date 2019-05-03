using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public string Name;
    public int CurrentRound;
    public int[] RoundScore = new int[21];
    void Start()
    {
        CurrentRound = 0;
    }

    public void AddScore(int score)
    {
        RoundScore[CurrentRound] = score;
    }
}
