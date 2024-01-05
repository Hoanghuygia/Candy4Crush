using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject tileScreen;
    public GameObject mainButton;
    public void BackToTileScreen() {
        tileScreen.SetActive(true);
        this.gameObject.SetActive(false);
    }
   
}
