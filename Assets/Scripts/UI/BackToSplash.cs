using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplash : MonoBehaviour
{
    public GameData gameData;
    public string sceneToLoad;
    private Board board;
    public void WinOK() {
        if(gameData != null) {
            gameData.saveData.ActiveLevel[board.level + 1] = true;
            //gameData.saveData.stars[board.level] = 3;
            gameData.Save();
        }
        SceneManager.LoadScene(sceneToLoad);
    }
    public void LoseOK() {

        SceneManager.LoadScene(sceneToLoad);

    }
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
