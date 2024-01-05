using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScreenPanel : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject SettingButton;
    public GameObject CancelSettingButton;
    public GameObject MoreInforButton;
    public GameObject BackMoreInforButton;
    public GameObject InforHolderPanel;
    public void TapToPlayButton() {
        this.gameObject.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void OpenSetting() {
        SettingButton.SetActive(false);
        CancelSettingButton.SetActive(true);
    }
    public void CancelSetting() {
        CancelSettingButton.SetActive(false);
        SettingButton.SetActive(true);
    }
    public void OpenMoreInfor() {
        InforHolderPanel.SetActive(true);
    }
    public void CloseMoreInforButton() {
        InforHolderPanel.SetActive(false);
    }

    
}
