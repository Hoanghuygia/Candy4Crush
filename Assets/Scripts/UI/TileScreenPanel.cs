using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScreenPanel : MonoBehaviour
{
    public SoundManagerSplash soundManagerSplash;
    public GameObject levelSelect;
    public GameObject SettingButton;
    public GameObject CancelSettingButton;
    public GameObject MoreInforButton;
    public GameObject BackMoreInforButton;
    public GameObject InforHolderPanel;
    public GameObject Sound_1;
    public GameObject Sound_1Cancel;
    public GameObject Sound_2;
    public GameObject Sound_2Cancel;
    private void Start() {
        soundManagerSplash = FindObjectOfType<SoundManagerSplash>();
    }

    public void Turn_offMusic() {
        soundManagerSplash.PlayClickNoise();
        soundManagerSplash.backGroundMusic.mute = true;
        Sound_2.SetActive(false);
        Sound_2Cancel.SetActive(true);
    }
    public void Turn_offSound() {
        soundManagerSplash.PlayClickNoise();
        soundManagerSplash.sound = false;
        Sound_1.SetActive(false);
        Sound_1Cancel.SetActive(true);
    }
    public void Turn_onMusic() {
        soundManagerSplash.PlayClickNoise();
        soundManagerSplash.backGroundMusic.mute = false;
        Sound_2Cancel.SetActive(false);
        Sound_2.SetActive(true);
    }
    public void Turn_onSound() {
        soundManagerSplash.PlayClickNoise();
        soundManagerSplash.sound = true;
        Sound_1.SetActive(true);
        Sound_1Cancel.SetActive(false);
    }
    public void TapToPlayButton() {
        soundManagerSplash.PlayClickNoise();
        this.gameObject.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void OpenSetting() {
        soundManagerSplash.PlayClickNoise();
        SettingButton.SetActive(false);
        CancelSettingButton.SetActive(true);
    }
    public void CancelSetting() {
        soundManagerSplash.PlayClickNoise();
        CancelSettingButton.SetActive(false);
        SettingButton.SetActive(true);
    }
    public void OpenMoreInfor() {
        soundManagerSplash.PlayClickNoise();
        InforHolderPanel.SetActive(true);
    }
    public void CloseMoreInforButton() {
        soundManagerSplash.PlayClickNoise();
        InforHolderPanel.SetActive(false);
    }

    
}
