using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject tileScreen;
    public GameObject mainButton;
    public SoundManagerSplash soundManagerSplash;

    private void Start() {
        soundManagerSplash = FindObjectOfType<SoundManagerSplash>();
    }
    public void BackToTileScreen() {
        soundManagerSplash.PlayClickNoise();
        tileScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }
   
}
