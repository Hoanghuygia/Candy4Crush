using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelControllers : MonoBehaviour
{
    public Animator paneAnim;
    public Animator gameInforAnim;

    public void TriggerOK() {
        if (paneAnim != null && gameInforAnim != null) {
            paneAnim.SetBool("Out", true);
            gameInforAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }

    }
    public void GameOver() {
        paneAnim.SetBool("Out", false);
        paneAnim.SetBool("Gave Over", true);
    }
    IEnumerator GameStartCo() {
        yield return new WaitForSeconds(1f);
        Board board = FindObjectOfType<Board>();
        board.currentState = GameState.move;
        Debug.Log("This is the main class!!!");
    }

}
