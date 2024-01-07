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
    public int[] scoreGoal;
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
        GetGoals();
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
            if(score > scoreGoal[0]) {
                gameData.saveData.stars[board.level] = 1;
            }
            if (score > scoreGoal[1]) {
                gameData.saveData.stars[board.level] = 2;
            }
            if (score > scoreGoal[2]) {
                gameData.saveData.stars[board.level] = 3;
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
    void GetGoals() {
        if (board != null) {
            if (board.world != null) {
                if (board.level < board.world.levels.Length) {
                    if (board.world.levels[board.level] != null) {
                        scoreGoal = board.world.levels[board.level].scoreGoals;
                    }
                }
            }
        }
    }

}
