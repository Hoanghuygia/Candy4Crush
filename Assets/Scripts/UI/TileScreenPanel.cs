using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScreenPanel : MonoBehaviour
{
    public GameObject levelSelect;
    public void TapToPlayButton() {
        this.gameObject.SetActive(false);
        levelSelect.SetActive(true);
    }

    
}
