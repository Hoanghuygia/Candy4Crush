using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board board;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board != null) {

            RepostionCamera(board.width, board.height);
        }
    }
    void RepostionCamera(float x, float y) {
        Vector2 temPosition = new Vector2(Mathf.Round(x/2), Mathf.Round(y/2));
        transform.position = temPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
