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
    public bool isClear;

    public TextMeshProUGUI scoreText;

    public int currentScore;
    public int clearScore = 1500;

    public TextMeshProUGUI timerText;

    public float gameTime = 0f;
    int seconds;

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
    public void AddScore()
    {
        currentScore += 100;
        clearedLines++;


        scoreText.text = "Score: " + currentScore.ToString();

        //Debug.Log(currentScore);

        if(currentScore >= clearScore) 
        {
            isClear = true;
            GameClear();
        }

    }
    public void GameOver()
    {
        PlayerPrefs.SetInt("Score", currentScore);
        PlayerPrefs.SetInt("ClearedLines", clearedLines);
        PlayerPrefs.SetFloat("Time", gameTime);
        PlayerPrefs.SetString("Result", "GameOver");

        SceneManager.LoadScene("ResultScene");

    }

    // GameClear‚µ‚½‚Ìˆ—
    // ¡‰ñ‚Ì’Ç‰Á
    public void GameClear()
    {

        PlayerPrefs.SetInt("Score", currentScore);
        PlayerPrefs.SetInt("ClearedLines", clearedLines);
        PlayerPrefs.SetFloat("Time", gameTime);
        PlayerPrefs.SetString("Result", "GameClear");

        SceneManager.LoadScene("ResultScene");


    }
    public void TimeManagement()
    {
        gameTime += Time.deltaTime;

        int minutes = (int)gameTime / 60;
        int seconds = (int)gameTime % 60;

        timerText.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
