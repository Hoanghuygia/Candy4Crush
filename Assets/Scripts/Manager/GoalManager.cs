using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal {
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoal;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    private EndGameManager endGameManager;
    private Board board;

    private void Start() {
        endGameManager = FindObjectOfType<EndGameManager>();
        board = FindObjectOfType<Board>();
        GetGoals();
        SetUpGoals();
    }
    void GetGoals() {
        if(board != null) {
            if(board.world != null) {
                if (board.level < board.world.levels.Length) {
                    if (board.world.levels[board.level] != null) {
                        levelGoal = board.world.levels[board.level].levelGoal;
                        for(int i = 0; i < levelGoal.Length; i++) {
                            levelGoal[i].numberCollected = 0;
                        }
                    }
                }
            }
        }
    }

    void SetUpGoals() {
        for(int i = 0; i< levelGoal.Length; i++) {
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);

            //Set the image and the text of the goal
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoal[i].goalSprite;
            panel.thisString = "0/" + levelGoal[i].numberNeeded;


            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform);

            panel = gameGoal.GetComponent<GoalPanel>();

            currentGoals.Add(panel);
            panel.thisSprite = levelGoal[i].goalSprite;
            panel.thisString = "0/" + levelGoal[i].numberNeeded;
        }
    }

    public void UpdateGoal() {
        int goalComplete = 0;
        for(int i = 0; i < levelGoal.Length;i++) {
            currentGoals[i].thisText.text = "" + levelGoal[i].numberCollected + "/" + levelGoal[i].numberNeeded;
            if (levelGoal[i].numberCollected >= levelGoal[i].numberNeeded) {
                goalComplete++;
                currentGoals[i].thisText.text = "" + levelGoal[i].numberNeeded + "/" + levelGoal[i].numberNeeded;
            }
        }
        if(goalComplete >= levelGoal.Length) {
            if(endGameManager != null) {
                endGameManager.WinGame();
            }
            //Debug.Log("You win!!1");
        }
    }
    public void CompareGoal(string goalToCompare) {
        for(int i = 0; i < levelGoal.Length; i++) {
            if(goalToCompare == levelGoal[i].matchValue) {
                levelGoal[i].numberCollected++;
            }
        }
    }
}
