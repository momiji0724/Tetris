using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Assemblies;

public class GameManagement : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public int currentScore;
    public int clearScore = 1500;

    public TextMeshProUGUI timerText;

    public float gameTime = 60f;
    int seconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        scoreText.text = "Score: " + currentScore.ToString();

        Debug.Log(currentScore);

        if(currentScore >= clearScore) 
        {
            GameClear();
        }

    }
    public void GameOver()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    // GameClear‚µ‚½Žž‚Ìˆ—
    // ¡‰ñ‚Ì’Ç‰Á
    public void GameClear()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void TimeManagement()
    {
        gameTime -= Time.deltaTime;
        seconds = (int)gameTime;

        if (seconds >= 0 && timerText != null)
        {
            timerText.text = seconds.ToString();
        }

        if (seconds <= 0)
        {
            Debug.Log("TimeOut");
            GameOver();
        }
    }
}
