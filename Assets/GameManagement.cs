using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assemblies;

public class GameManagement : MonoBehaviour
{
    public int clearedLines;
    public TextMeshProUGUI scoreText;
    public int currentScore;

    [Header("Level Settings")]
    public TextMeshProUGUI levelText;
    public int currentLevel = 1;

    public TextMeshProUGUI timerText;
    public float gameTime = 0f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mino.ClearGrid();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        TimeManagement();
    }
    private void Initialize()
    {
        currentScore =0;
    }
    public void AddScore(int lines)
    {
        if (lines <= 0) return;

        currentScore += lines * 100;
        clearedLines += lines;


        scoreText.text = "Score: " + currentScore.ToString();

        //Debug.Log(currentScore);

        // 10列消すとゲームレベルがアップ
        int newLevel = (clearedLines / 10) + 1;

        if(newLevel > currentLevel) 
        {
            currentLevel = newLevel;
            UpDateLevelUI();
            Debug.Log($"レベルアップ！現在のレベル:{currentLevel}");

            Mino activeMino = FindObjectOfType<Mino>();
            if(activeMino != null) 
            {
                activeMino.UpdateFallTimeByLevel(currentLevel);
            }
            
        }

    }
    private void UpDateLevelUI() 
    {
        if(levelText != null) 
        {
            levelText.text = "Level: " + currentLevel.ToString();
        }
    }
    public void GameOver()
    {
        Mino actionMino = FindObjectOfType<Mino>();
        if(actionMino != null) 
        {
            actionMino.enabled = false;
        }

        StartCoroutine(Mino.ChangeGridToGrayAnimation(() =>
        {
            PlayerPrefs.SetInt("Score", currentScore);
            PlayerPrefs.SetInt("ClearedLines", clearedLines);
            PlayerPrefs.SetInt("Level", currentLevel);
            PlayerPrefs.SetFloat("Time", gameTime);
            PlayerPrefs.SetString("Result", "GameOver");

            SceneManager.LoadScene("ResultScene");
        }));

    }

    // GameClearした時の処理
    // 今回の追加
    public void TimeManagement()
    {
        gameTime += Time.deltaTime;

        int minutes = (int)gameTime / 60;
        int seconds = (int)gameTime % 60;

        timerText.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
