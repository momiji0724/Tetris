using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Header("Result Text Components")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lineScore;

    [Header("Save UI Components")]
    public TMP_InputField nameInputField;
    public Button saveButton;

    [Header("Ranking UI Components")]
    public TextMeshProUGUI[] rankingTexts = new TextMeshProUGUI[5];

    private int currentScore;
    private int currentLines;
    private float currentTime;
    private int currentLevel;

    private const string RankingKey = "GameRanking";
    private const int MaxRankingCount = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string result = PlayerPrefs.GetString("Result");
        currentScore = PlayerPrefs.GetInt("Score");
        currentLevel = PlayerPrefs.GetInt("Level",1);
        currentLines = PlayerPrefs.GetInt("ClearedLines");
        currentTime = PlayerPrefs.GetFloat("Time");


        int minutes = (int)currentTime / 60;
        int seconds = (int)currentTime % 60;

        titleText.text = result + "!";

        timeText.text =
            "Time : " +
            minutes.ToString("00") +
            " : " +
            seconds.ToString("00");

        scoreText.text = "Score : " + currentScore.ToString();

        if(levelText != null) 
        {
            levelText.text = "Final Level : "+ currentLevel.ToString();
        }

        lineScore.text = "ClearedLines : " + currentLines.ToString();

        if(saveButton != null) 
        {
            saveButton.onClick.AddListener(SaveCurrentScore);
        }
    }

    public void SaveCurrentScore()
    {
        // 1. 連続で押されないように、メソッドに入った瞬間にまずボタンを無効化する
        if (saveButton != null)
        {
            saveButton.interactable = false;
        }

        string inputName = nameInputField != null ? nameInputField.text : "";
        if (string.IsNullOrEmpty(inputName))
        {
            inputName = "No Name";
        }

        ScoreRecord newRecord = new ScoreRecord();
        newRecord.userName = inputName;
        newRecord.score = currentScore;
        newRecord.clearedLines = currentLines;
        newRecord.timer = currentTime;
        newRecord.finalLevel = currentLevel;
        newRecord.dateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

        RankingWrapper rankingData = LoadRanking();
        rankingData.rankingList.Add(newRecord);
        rankingData.rankingList.Sort((a, b) => b.score.CompareTo(a.score));

        if (rankingData.rankingList.Count > MaxRankingCount)
        {
            rankingData.rankingList.RemoveRange(MaxRankingCount, rankingData.rankingList.Count - MaxRankingCount);
        }

        string jsonString = JsonUtility.ToJson(rankingData);
        PlayerPrefs.SetString(RankingKey, jsonString);
        PlayerPrefs.Save();

        Debug.Log("スコアを保存しました！リザルト画面に留まります。");

    }

    private RankingWrapper LoadRanking() 
    {
        if (PlayerPrefs.HasKey(RankingKey)) 
        {
            string jsonString = PlayerPrefs.GetString(RankingKey);
            return JsonUtility.FromJson<RankingWrapper>(jsonString);
        }
        return new RankingWrapper();
    }


    public void GotoRankingScene() 
    {
        SceneManager.LoadScene("RankingScene");
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScene");

    }

    // Update is called once per frame
    void Update()
    {
        if (nameInputField != null && nameInputField.isFocused) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }

        // Escapeキーが押されたらゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
}
