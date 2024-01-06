using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButtonController : MonoBehaviour
{
    private Board board;
    public GameObject settingHolder;
    public GameObject turnOffSound;
    public GameObject turnOnSound;
    public GameObject turnOffMusic;
    public GameObject turnOnMusic;
    // Start is called before the first frame update
    void Start()
    {
        board = FindAnyObjectByType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickTurnOnSetting() {
        this.gameObject.SetActive(false);
        settingHolder.SetActive(true);
    }
    public void ClickTurnOffSetting() {
        settingHolder.SetActive(false);
        this.gameObject.SetActive(true);
    }
    public void TurnOffSound() {
        board.sound = false;
        turnOffSound.SetActive(false);
        turnOnSound.SetActive(true);
    }
    public void TurnOnSound() {
        board.sound = true;
        turnOffSound.SetActive(true);
        turnOnSound.SetActive(false);

    }
    public void TurnOffMusic() {
        board.music = false;
        turnOffMusic.SetActive(false);
        turnOnMusic.SetActive(true);
    }
    public void TurnOnMusic() {
        board.music = true;
        turnOnMusic.SetActive(false);
        turnOffMusic.SetActive(true);
    }
}
