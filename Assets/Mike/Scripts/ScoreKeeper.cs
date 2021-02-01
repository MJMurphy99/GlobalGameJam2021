using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text score;
    public Text highScore;
    public static int playerScoreNum = 0;
    public static int playerHighScoreNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerScoreNum = 0;
        playerHighScoreNum = 0;
        score.text = "Score: " + ScoreKeeper.playerScoreNum;
        highScore.text = "High Score: " + ScoreKeeper.playerHighScoreNum;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + ScoreKeeper.playerScoreNum;

        if (playerHighScoreNum > playerScoreNum)
        {
            highScore.text = "High Score: " + ScoreKeeper.playerHighScoreNum;
        }
        else
        {
            highScore.text = "High Score: " + ScoreKeeper.playerScoreNum;
        }

        //Debug.Log(playerHighScoreNum);
    }
}
