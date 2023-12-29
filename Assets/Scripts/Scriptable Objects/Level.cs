using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]

public class Level : ScriptableObject
{
    [Header("Board Dimensions")]
    public int width;
    public int height;

    [Header("Starting tiles")]
    public TypeTile[] boardLayout;

    [Header("Availble dots")]
    public GameObject[] dots;

    [Header("Score goals")]
    public int[] scoreGoals;

    [Header("End game requirement")]
    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoal;
}
