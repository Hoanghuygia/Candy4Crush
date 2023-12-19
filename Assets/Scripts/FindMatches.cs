using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }
    public void FindAllMatches() {
        StartCoroutine(FindAllMatchesCo());
    }
    private IEnumerator FindAllMatchesCo() {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i< board.width; i++) {
            for(int j = 0; j < board.height; j++){
                GameObject currentDot = board.allDots[i, j];
                if(currentDot != null) {
                    if(i > 0 && i < board.width - 1) {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot != null && rightDot != null) { 
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) {

                                if (currentDot.GetComponent<Dot>().RowBomb
                                    || leftDot.GetComponent<Dot>().RowBomb
                                    || rightDot.GetComponent<Dot>().RowBomb) {
                                    currentMatches.Union(GetRowPieces(j));
                                }
                                if (currentDot.GetComponent<Dot>().ColumnBomb) {
                                    currentMatches.Union(GetColumnPieces(i));
                                }
                                if (leftDot.GetComponent<Dot>().ColumnBomb) {
                                    currentMatches.Union(GetColumnPieces(i - 1));
                                }
                                if (rightDot.GetComponent<Dot>().ColumnBomb) {
                                    currentMatches.Union(GetColumnPieces(i + 1));
                                }
                                if (!currentMatches.Contains(leftDot) ) {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().Matched = true;
                                if (!currentMatches.Contains(rightDot)) {
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().Matched = true;
                                if (!currentMatches.Contains(currentDot)) {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().Matched = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1) {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if (upDot != null && downDot != null) {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) {

                                if (currentDot.GetComponent<Dot>().ColumnBomb
                                    || upDot.GetComponent<Dot>().ColumnBomb
                                    || downDot.GetComponent<Dot>().ColumnBomb) {
                                    currentMatches.Union(GetColumnPieces(i));
                                }
                                if (currentDot.GetComponent<Dot>().RowBomb) {
                                    currentMatches.Union(GetRowPieces(j));
                                }
                                if (upDot.GetComponent<Dot>().RowBomb) {
                                    currentMatches.Union(GetRowPieces(j + 1));
                                }
                                if (downDot.GetComponent<Dot>().RowBomb) {
                                    currentMatches.Union(GetRowPieces(j - 1));
                                }
                                if (!currentMatches.Contains(upDot)) {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().Matched = true;
                                if (!currentMatches.Contains(downDot)) {
                                    currentMatches.Add(downDot);            
                                }
                                downDot.GetComponent<Dot>().Matched = true;
                                if (!currentMatches.Contains(currentDot)) {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().Matched = true;
                            }
                        }
                    }
                }
            }
        }
    }
    List<GameObject> GetColumnPieces(int column) {
        List<GameObject> dots = new List<GameObject>();
        for(int i = 0; i< board.height; i++) {
            if (board.allDots[column, i] != null) {
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().Matched = true;
            }
        }
        return dots;
    }
    List<GameObject> GetRowPieces(int row) {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++) {
            if (board.allDots[i, row] != null) {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().Matched = true;
            }
        }
        return dots;
    }
    public void CheckBombs() {
        //did the player move something?
        if (board.currentDot != null)
        {//is the piece they move matched?
            if (board.currentDot.Matched) {         //there are two case that we swipe a true match, swipe this match or click the other dot

                board.currentDot.Matched = false;//we set it to unmatched since we want it to stay and decide what kind of bomb it is
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb > 50) {
                    board.currentDot.MakeColumnBomb();
                }
                else {
                    //make row bomb
                    board.currentDot.MakeRowBomb();
                }
            }
            else if(board.currentDot.otherDot != null) {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                //Dot otherDot = board.currentDot.GetComponent<Dot>().otherDot.GetComponent<Dot>();//the way he uses
                if (otherDot.Matched) {
                    otherDot.Matched = false;
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb > 50) {
                        //make colum bomb
                        otherDot.MakeColumnBomb();
                    }
                    else {
                        //make row bomb
                        otherDot.MakeRowBomb();
                    }
                }
            }
        }
    }

}
