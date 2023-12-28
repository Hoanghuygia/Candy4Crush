using System.Collections;
using System.Collections.Generic;
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
    public EndGameRequirements requirements;
    public GameObject movesLabel;
    public GameObject timeLabel;
    public Text counter;

    public int currentCounterValue;
    private float timerSecond;

    public void Start() {
        SetUpGame();
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
        if(currentCounterValue >= 1) {

            currentCounterValue--;
            counter.text = "" + currentCounterValue;
        }
        else {
            Debug.Log("You lose111");
        }

    }
    public void Update() {
        if(requirements.gameType == GameType.Time) {
            timerSecond -= Time.deltaTime;
            if(timerSecond <= 0) {
                DecreaseCounterValue();
                timerSecond = 1;
            }
        }
    }
}
