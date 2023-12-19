using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int prevousColumn;
    public int prevousRow;
    public int targetX;
    public int targetY;
    public bool Matched = false;

    public GameObject otherDot;
    private FindMatches findMatches;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    [Header ("Swipe stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Power Up")]
    public bool ColumnBomb;
    public bool RowBomb;
    public bool ColorBomb;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;

    // Start is called before the first frame update
    void Start()
    {
        ColumnBomb = false;
        RowBomb = false;
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //prevousColumn = column;
        //prevousRow = row;
    }

    // Update is called once per frame

    //This is for testing and debug only.
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            ColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
        }
    }
    void Update()
    {
        //FindMatches();
        //if (Matched) {
        //    SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        //    mySprite.color = new Color(1f, 1f, 1f, .2f);
        //}
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move toward the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move toward the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();

        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }

    }
    public IEnumerator CheckMoveCo()            //this function is to make the pieces back the prevous place or destroy 
    {
        yield return new WaitForSeconds(.5f);
        if(otherDot != null)        //why we have to check whether it is null or not
        {
            if (ColorBomb) {
                findMatches.MatchPiecesOfColor(otherDot.tag);//it means that it would destroy the color when two pieces has the same color
                //this piece is a color bomb and the other piece is the color to destroy
                Matched = true;
            }
            else if (otherDot.GetComponent<Dot>().ColorBomb) {
                //the other dot is the color bomb
                findMatches.MatchPiecesOfColor(this.gameObject.tag);
                otherDot.GetComponent<Dot>().Matched = true;
            }
            if(!Matched && !otherDot.GetComponent<Dot>().Matched)
            {
                otherDot.GetComponent<Dot>().column = column;
                otherDot.GetComponent<Dot>().row = row;
                row = prevousRow;
                column = prevousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
                
            }
            otherDot = null;        //we want to set the null value to null since it can affect other swipe later
        }
        
    }
    private void OnMouseDown(){
        if(board.currentState == GameState.move) {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void OnMouseUp(){
        if(board.currentState == GameState.move) {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }
    void CalculateAngle()       //fix bug that it automatically move pieces when touching
    {
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist){

            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;
            board.currentDot = this;            //keep track the current dot - the dot that is clicked

        }
        else {
            board.currentState = GameState.move;
        }
        

    }
    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right swipe
            otherDot = board.allDots[column + 1, row];//this line is to take the other dot
            prevousColumn = column;             //save the prevous location here instead of 
            prevousRow = row;
            otherDot.GetComponent<Dot>().column -= 1;//this line is to change the position of the other dot
            column += 1;//this line is to change the column of the current dot

        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Upper swipe
            otherDot = board.allDots[column, row + 1];
            prevousColumn = column;
            prevousRow = row;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;

        }
        else if ((swipeAngle > 135 || swipeAngle <= -135)  && column > 0)
        {
            //Left swipe
            otherDot = board.allDots[column - 1, row];
            prevousColumn = column;
            prevousRow = row;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;

        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Bottom swipe
            otherDot = board.allDots[column, row - 1];
            prevousColumn = column;
            prevousRow = row;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;

        }
        StartCoroutine(CheckMoveCo());
    }
    void FindMatches()          //why we do not need to loop for each of the pieces, that is because the dot class always run each loop in board class
    {
        if(column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            //add tag to each dot means that each dot in the prefab has each own unique name, such like it
            if(leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().Matched = true;
                    rightDot1.GetComponent<Dot>().Matched = true;
                    Matched = true;
                }
            }
            
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if(upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().Matched = true;
                    downDot1.GetComponent<Dot>().Matched = true;
                    Matched = true;
                }
            }
            //add tag to each dot means that each dot in the prefab has each own unique name, such like it
            
        }
    }
    public void MakeRowBomb() {
        RowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }
    public void MakeColumnBomb() {
        ColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }
}
