using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lineScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string result = PlayerPrefs.GetString("Result");
        int score = PlayerPrefs.GetInt("Score");
        int lines = PlayerPrefs.GetInt("ClearedLines");
        float time = PlayerPrefs.GetFloat("Time");

        int minutes = (int)time / 60;
        int seconds = (int)time % 60;

        titleText.text = result + "!";

        timeText.text =
            "Time : " +
            minutes.ToString("00") +
            " : " +
            seconds.ToString("00");

        scoreText.text = "Score : " + score.ToString();

        lineScore.text = "ClearedLines : " + lines.ToString();
    }
    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
