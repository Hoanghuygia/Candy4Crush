using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class ConfirmPanel : MonoBehaviour
{
    public string levelToLoad;
    public Image[] stars;
    private int startsActive;
    public int level;
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindAnyObjectByType<GameData>();
        LoadData();         //i think that we could reference it to the level button class instead
        ActiveStars();
    }
    void LoadData() {
        if(gameData != null) {
            startsActive = gameData.saveData.stars[level - 1];
        }
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
