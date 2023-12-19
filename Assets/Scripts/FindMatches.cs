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
    private List<GameObject> AdjacentBomb(GameObject dot1,  GameObject dot2, GameObject dot3) {
        Dot dot1Dot = dot1.GetComponent<Dot>();
        Dot dot2Dot = dot2.GetComponent<Dot>();
        Dot dot3Dot = dot3.GetComponent<Dot>();
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1Dot.AdjacentBomb) {
            currentMatches.Union(GetAdjacentPieces(dot1Dot.column, dot1Dot.row));
        }
        if (dot2Dot.AdjacentBomb) {
            currentMatches.Union(GetAdjacentPieces(dot2Dot.column, dot2Dot.row));
        }
        if (dot3Dot.AdjacentBomb) {
            currentMatches.Union(GetAdjacentPieces(dot3Dot.column, dot3Dot.row));
        }

        return currentDots;
    }
    private List<GameObject> RowBomb(GameObject dot1, GameObject dot2, GameObject dot3) {
        Dot dot1Dot = dot1.GetComponent<Dot>();
        Dot dot2Dot = dot2.GetComponent<Dot>();
        Dot dot3Dot = dot3.GetComponent<Dot>();
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1Dot.RowBomb) {
            currentMatches.Union(GetRowPieces(dot1Dot.row));
        }
        if (dot2Dot.RowBomb) {
            currentMatches.Union(GetRowPieces(dot2Dot.row));
        }
        if (dot3Dot.RowBomb) {
            currentMatches.Union(GetRowPieces(dot3Dot.row));
        }

        return currentDots;
    }
    private List<GameObject> ColumnBomb(GameObject dot1, GameObject dot2, GameObject dot3) {
        Dot dot1Dot = dot1.GetComponent<Dot>();
        Dot dot2Dot = dot2.GetComponent<Dot>();
        Dot dot3Dot = dot3.GetComponent<Dot>();
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1Dot.ColumnBomb) {
            currentMatches.Union(GetColumnPieces(dot1Dot.column));
        }
        if (dot2Dot.ColumnBomb) {
            currentMatches.Union(GetColumnPieces(dot2Dot.column));
        }
        if (dot3Dot.ColumnBomb) {
            currentMatches.Union(GetColumnPieces(dot3Dot.column));
        }

        return currentDots;
    }
    private void AddToListAndMatch(GameObject dot) {
        if (!currentMatches.Contains(dot)) {
            currentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().Matched = true;
    }
    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3) {//why use game object instead of dot
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }
    private IEnumerator FindAllMatchesCo() {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i< board.width; i++) {
            for(int j = 0; j < board.height; j++){
                GameObject currentDot = board.allDots[i, j];
                //Dot curretnDotDuplicate = currentDot.GetComponent<Dot>();
                if(currentDot != null) {
                    if(i > 0 && i < board.width - 1) {
                        GameObject leftDot = board.allDots[i - 1, j];
                        //Dot leftDotDuplicate = leftDot.GetComponent<Dot>();
                        GameObject rightDot = board.allDots[i + 1, j];
                        //Dot rightDotDuplicate = rightDot.GetComponent<Dot>();
                        if(leftDot != null && rightDot != null) { 
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) {

                                currentMatches.Union(RowBomb(leftDot, currentDot, rightDot));
                                currentMatches.Union(ColumnBomb(leftDot, currentDot, rightDot));
                                currentMatches.Union(AdjacentBomb(leftDot, currentDot, rightDot));
                                GetNearbyPieces(leftDot, currentDot, rightDot);
                                
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1) {
                        GameObject upDot = board.allDots[i, j + 1];
                        //Dot upDotDuplicate = upDot.GetComponent<Dot>();
                        GameObject downDot = board.allDots[i, j - 1];
                        //Dot downDotDuplicate = downDot.GetComponent<Dot>();
                        if (upDot != null && downDot != null) {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) {

                                currentMatches.Union(ColumnBomb(upDot, currentDot, downDot));
                                currentMatches.Union(RowBomb(upDot, currentDot, downDot));
                                currentMatches.Union(AdjacentBomb(upDot, currentDot, downDot));

                                GetNearbyPieces(upDot, currentDot, downDot);
                                
                            }
                        }
                    }
                }
            }
        }
    }
    public void MatchPiecesOfColor(string color) {
        for(int i = 0; i < board.width; i++) {
            for(int j = 0; j < board.height; j++) {
                if (board.allDots[i, j] != null) {
                    if (board.allDots[i, j].tag == color) {
                        board.allDots[i, j].GetComponent<Dot>().Matched = true;//since this is only a GameObject, so we have to access to its script
                    }
                }
            }
        }
    }
    List<GameObject> GetAdjacentPieces(int column, int row) {
        List<GameObject> dots = new List<GameObject>();
        for(int i = column - 1; i <= column + 1; i++) {
            for(int j = row - 1; j <= row + 1; j++) {
                if (i >= 0 && i < board.width && j >= 0 && j < board.height){//make sure that the dot inside the board
                    dots.Add(board.allDots[i, j]);
                    board.allDots[i, j].GetComponent<Dot>().Matched = true;
                }
            }
        }
        return dots;
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
