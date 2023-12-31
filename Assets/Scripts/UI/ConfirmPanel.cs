using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class ConfirmPanel : MonoBehaviour
{
    [Header("Level Information")]
    public string levelToLoad;
    public int level;
    private GameData gameData;
    private int startsActive;
    private int highScore;

    [Header("UI stuff")]
    public Image[] stars;
    public Text highScoreText;
    public Text starText;

    void Start()
    {
        gameData = FindAnyObjectByType<GameData>();
        LoadData();         //i think that we could reference it to the level button class instead
        ActiveStars();
        SetText();
    }
    void LoadData() {
        if(gameData != null) {
            startsActive = gameData.saveData.stars[level - 1];
            highScore = gameData.saveData.highScore[level - 1];
        }
    }
    void SetText() {
        highScoreText.text = "" + highScore;
        starText.text = "" + startsActive + "/3";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ActiveStars() {
        for (int i = 0; i < startsActive; i++) {
            stars[i].enabled = true;
        }
    }
    public void Cancel() {
        this.gameObject.SetActive(false);
    }
    public void Play() {
        PlayerPrefs.SetInt("Current Level", level - 1);
        SceneManager.LoadScene(levelToLoad);
    }
}
