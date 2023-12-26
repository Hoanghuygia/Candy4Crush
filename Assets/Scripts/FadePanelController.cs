using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{
    public Animator paneAnim;
    public Animator gameInforAnim;

    public void TriggerOK() {
        if(paneAnim != null && gameInforAnim != null) {
            paneAnim.SetBool("Out", true);
            gameInforAnim.SetBool("Out", true);
        }
        
    }
    
}
