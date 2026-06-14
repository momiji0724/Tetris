using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingSceneManager : MonoBehaviour
{
    [Header("Ranking UI Components")]
    public TextMeshProUGUI[] rankingTexts = new TextMeshProUGUI[5];

    private const string RankingKey = "GameRanking";
    private const int MaxRankingCount = 5;
    private void DisplayRanking()
    {
        // 既存のランキングをロード
        RankingWrapper data = new RankingWrapper();
        if (PlayerPrefs.HasKey(RankingKey))
        {
            string jsonString = PlayerPrefs.GetString(RankingKey);
            data = JsonUtility.FromJson<RankingWrapper>(jsonString);
        }

        // UIテキストの書き換え
        for (int i = 0; i < MaxRankingCount; i++)
        {
            if (rankingTexts[i] == null) continue;

            if (i < data.rankingList.Count)
            {
                ScoreRecord r = data.rankingList[i];
                rankingTexts[i].text = $"{i + 1}位 : {r.dateTime} - {r.userName} - {r.score}点 (Lv.{r.finalLevel})";
            }
            else
            {
                rankingTexts[i].text = $"{i + 1}位 : -----";
            }
        }
    }

    public void BackToTitle() 
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void ClearRankingData() 
    {
        if (PlayerPrefs.HasKey(RankingKey))
        {
            PlayerPrefs.DeleteKey(RankingKey);
            PlayerPrefs.Save();
            Debug.Log("ランキングデータを削除しました。");
        }

        // 2. 画面上の表示を即座にリセット（再度ロードして描画）
        DisplayRanking();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplayRanking();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BackToTitle();
        }
    }
}
