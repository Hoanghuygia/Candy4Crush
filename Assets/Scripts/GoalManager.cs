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
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;

    private void Start() {
        SetUpIntroGoals();
    }

    void SetUpIntroGoals() {
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
            panel.thisSprite = levelGoal[i].goalSprite;
            panel.thisString = "0/" + levelGoal[i].numberNeeded;
        }
    }
}