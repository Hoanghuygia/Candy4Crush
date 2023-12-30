using System.Collections;
using System.Collections.Generic;
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
    //public Text levelText;


    public Image[] starts;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        ActiveStar();
        GetLevel();
        DecideSprite();
    }
    void ActiveStar() {
        for(int i = 0; i <  starts.Length; i++) {
            starts[i].enabled = false;
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
