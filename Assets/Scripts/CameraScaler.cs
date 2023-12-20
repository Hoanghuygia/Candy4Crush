using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board board;
    public float cameraOffet;
    public float aspectRatio = 0.625f;
    public float padding = 2;       //the ratio between aspectRatio and padding have to equals to the ratio of the camera (this case is 16-9)
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board != null) {

            RepostionCamera(board.width - 1, board.height - 1);
        }
    }
    void RepostionCamera(float x, float y) {
        Vector3 temPosition = new Vector3(x/2, y/2, cameraOffet);
        transform.position = temPosition;
        if(board.width >= board.height) {
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        }
        else {
            Camera.main.orthographicSize = board.height / 2 + padding;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
