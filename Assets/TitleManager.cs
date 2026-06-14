using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame() 
    {
        SceneManager.LoadScene("MainScene");
    }
    public void OpenRanking()
    {
        SceneManager.LoadScene("RankingScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Unityエディタ上での実行時は再生モードを終了する
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドしたゲームではアプリを終了する
        Application.Quit();
#endif
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
