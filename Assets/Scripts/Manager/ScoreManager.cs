using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Image scoreBar;
    public Text scoreText;
    public int score;
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        updateBar();
        
    }
    public void IncreaseScore(int amountToIncrease) {
        score += amountToIncrease;
        if (gameData != null) {
            if (score > gameData.saveData.highScore[board.level]) {
                gameData.saveData.highScore[board.level] = score;
            }
            gameData.Save();
        }
        if (board != null && scoreBar != null ) {
            int length = board.scoreGoals.Length;
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }
        
    }
    private void updateBar() {
        if(scoreBar.fillAmount >= 1) {
            scoreBar.fillAmount = 0;
        }
    }

}
