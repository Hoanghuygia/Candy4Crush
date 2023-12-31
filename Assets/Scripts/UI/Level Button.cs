using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Active stuff")]
    public bool Actice;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;
    private int starsActive;
    //public Text levelText;


    [Header("Level UI")]
    public Image[] starts;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;

    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        LoadData();
        ActiveStar();
        GetLevel();
        DecideSprite();
    }
    void LoadData() {
        if(gameData != null) {
            if (gameData.saveData.ActiveLevel[level - 1]) {
                Actice = true;
            }
            else {
                Actice = false;
            }
            //decide how many stars we should have
            starsActive = gameData.saveData.stars[level - 1];
        }
    }
    void ActiveStar() {
        for(int i = 0; i < starsActive; i++) {

            starts[i].enabled = true;
        }
    }
    void DecideSprite() {
        if (Actice) {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }
        else {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
        
    }
    void GetLevel() {
        levelText.text = "" + level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ComfirmPanel(int level) {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }
}
