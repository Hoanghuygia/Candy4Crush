using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Image[] starts;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ComfirmPanel() {
        confirmPanel.SetActive(true);
    }
}
