using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;


public enum GameType {
    Moves,
    Time
}
[System.Serializable]
public class EndGameRequirements {
    public GameType gameType;
    public int counterValue;
}
public class EndGameManager : MonoBehaviour
{
    public Board board;
    public EndGameRequirements requirements;
    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject winPanel;
    public GameObject tryAgainPanel;
    public Text counter;

    public int currentCounterValue;
    private float timerSecond;

    public void Start() {
        board = FindObjectOfType<Board>();
        SetGameType();
        SetUpGame();
    }
    void SetGameType() {
        if (board.level < board.world.levels.Length) {
            if (board.world != null) {
                if (board.world.levels[board.level] != null) {
                    requirements = board.world.levels[board.level].endGameRequirements;
                }
            }
        }
    }

    public void SetUpGame() {
        currentCounterValue = requirements.counterValue;
        if(requirements.gameType == GameType.Moves) {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else {
            timerSecond = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }
    public void DecreaseCounterValue() {
        if(board.currentState == GameState.move) {         //i think that we need to let it more specific by ==move, he use != pause
            currentCounterValue--;
            counter.text = "" + currentCounterValue;

            if (currentCounterValue <= 0) {
                LoseGame();
            }
        }
    }
    public void LoseGame() {
        tryAgainPanel.SetActive(true);
        Debug.Log("You lose111");
        board.currentState = GameState.lose;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelControllers fade = FindAnyObjectByType<FadePanelControllers>();
        fade.GameOver();
    }
    public void WinGame() {
        winPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelControllers fade = FindAnyObjectByType<FadePanelControllers>();
        fade.GameOver();
    }
    public void Update() {
        if(requirements.gameType == GameType.Time && currentCounterValue > 0) {
            timerSecond -= Time.deltaTime;
            if(timerSecond <= 0) {
                DecreaseCounterValue();
                timerSecond = 1;
            }
        }
    }
}
