using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    public Text ScoreText;
    public Text LifesText;
    public ScreenFader ScreenFader;
    public GameObject StartMessageWindow;
    public GameObject ScorePanel;
    public Text PlayerNameInputField;
    public GameObject ScoreTable;
    public GameObject ScorePref;

    private void Start()
    {
        UpdateScoreText();
        UpdateLifes();
    }

    public void UpdateScoreText()
    {
        ScoreText.text = "Score: "+GameController.Instance.CurrentScore.ToString();
    }

    public void UpdateLifes()
    {
        LifesText.text = "Lifes: " + GameController.Instance.Lifes.ToString();
    }

    public void UpdateScoreTable(List<PlayerData> players)
    {
        foreach (PlayerData player in players)
        {
            Text playerInfo = Instantiate(ScorePref, ScoreTable.transform).GetComponent<Text>();
            playerInfo.text = player.Name + ": " + player.Score;
        }
    }
}
