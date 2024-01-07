using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEditor.U2D.Sprites;

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

    [Header("Sound stuff")]
    public SoundManagerSplash soundManagerSplash;
    private int sound;
    private int music;


    void OnEnable()
    {
        gameData = FindAnyObjectByType<GameData>();
        LoadData();         //i think that we could reference it to the level button class instead
        ActiveStars();
        SetText();
    }
    private void Start() {
        soundManagerSplash = FindObjectOfType<SoundManagerSplash>();
        sound = soundManagerSplash.sound ? 1 : 0;
        music = soundManagerSplash.music ? 1 : 0;
    }
    void LoadData() {//why we do not load the stars
        if(gameData != null) {
            startsActive = gameData.saveData.stars[level - 1];
            highScore = gameData.saveData.highScore[level - 1];
        }
    }
    void SetText() {
        highScoreText.text = "" + highScore;
        starText.text = "" + startsActive + "/3";
    }
    void ActiveStars() {
        for (int i = 0; i < startsActive; i++) {
            stars[i].enabled = true;
        }
    }
    public void Cancel() {
        soundManagerSplash.PlayClickNoise();
        this.gameObject.SetActive(false);
    }
    public void Play() {
        soundManagerSplash.PlayClickNoise();
        PlayerPrefs.SetInt("Current Level", level - 1);         //this line is to load to other screen
        PlayerPrefs.SetInt("Current Music", music);
        PlayerPrefs.SetInt("Current Sound", sound);
        SceneManager.LoadScene(levelToLoad);
    }
}
