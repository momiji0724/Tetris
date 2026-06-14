using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ScoreRecord 
{
    public string userName;
    public string dateTime;
    public float score;
    public float timer;
    public int finalLevel;
    public int clearedLines;

}

[Serializable]
public class RankingWrapper 
{
    public List<ScoreRecord> rankingList = new List<ScoreRecord>();
}
public class SaveData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
