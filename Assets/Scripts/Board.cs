using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public enum GameState {
    wait,
    move
}
public class Board : MonoBehaviour{

    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject destroyEffect;
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;
    public Dot currentDot;
    private FindMatches findMatches;

    void Start(){
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }
    private void SetUp()
    {
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                Vector2 temptPosition = new Vector2(i, j + offSet);
                GameObject backgroundTile = Instantiate(tilePrefab, temptPosition, Quaternion.identity) as GameObject; 
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);
                int maxInteration = 0;
                while(MatchesAt(i, j, dots[dotToUse]) && maxInteration < 100) //why we need maxInteration ?
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxInteration++;
                }
                maxInteration = 0;
                GameObject dot = Instantiate(dots[dotToUse], temptPosition, Quaternion.identity);

                dot.GetComponent<Dot>().row = j;                              //make the slicing transition
                dot.GetComponent<Dot>().column = i;
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;
            }
        }
    }
    private bool MatchesAt(int column, int row, GameObject piece)       //could we have only one paramether piece ?
    {                                                                   //why piece.column is not ok
        if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) return true;
            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) return true;
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) return true;
            }
            if(column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) return true;
            }
        }
        return false;
    }
    //private bool MatchesAt(GameObject piece)
    //{
    //    if (piece.column > 1 && piece.row > 1)
    //    {
    //        if (allDots[column - 1, row].tag == piece.tag || allDots[column - 2, row].tag == piece.tag) return true;
    //        if (allDots[column, row - 1].tag == piece.tag || allDots[column, row - 2].tag == piece.tag) return true;
    //        piece.
    //    }
    //    return false;
    //}
    private bool ColumnOrRow() {
        int numberHorizotial = 0;
        int numberVertotial = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece == null) {
            foreach(GameObject currentPiece in findMatches.currentMatches) {
                Dot dot = currentPiece.GetComponent<Dot>();
                if(dot.row == firstPiece.row) {
                    numberHorizotial++;
                }
                if(dot.column == firstPiece.column) {
                    numberVertotial++;
                }
            }
        }
        return (numberVertotial == 5 || numberHorizotial == 5);
    }
    private void CheckToMakeBomb() {
        if(findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7) {
            findMatches.CheckBombs();//destroy all the row/column
        }
        if(findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8) {
            if (ColumnOrRow()) {
                //make a color bomb
                Debug.Log("Make a color bomb");
                //is the current dot matched
                if(currentDot != null) {
                    if (currentDot.Matched) {
                        if (!currentDot.ColorBomb) {
                            currentDot.Matched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else {//i dont know this case
                        if(currentDot.otherDot != null) {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.Matched) {
                                if(!otherDot.ColorBomb) {
                                    otherDot.Matched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }
                    }
                }
            }
            else {
                //make a adjacent bomb
                Debug.Log("Make a adjacent bomb");
                if (currentDot != null) {
                    if (currentDot.Matched) {
                        if (!currentDot.AdjacentBomb) {
                            currentDot.Matched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                    else {//i dont know this case
                        if (currentDot.otherDot != null) {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.Matched) {
                                if (!otherDot.AdjacentBomb) {
                                    otherDot.Matched = false;
                                    otherDot.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().Matched)       //if we want to change/access properties of other class, must use GetComponent()
        {
            if(findMatches.currentMatches.Count >= 4) {//why we need to check 7 dots
                CheckToMakeBomb();
            }
            findMatches.currentMatches.Remove(allDots[column, row]);     //each time we destroy the matches, also remove from the list
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f); 
            Destroy(allDots[column, row]);          //this code is to destroy the game object 
            allDots[column, row] = null;
        }
    }
    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo() {
        int nullCount = 0;
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if (allDots[i, j] == null) {
                    nullCount++;
                }
                else if(nullCount > 0) {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;     //this means that it would iterate to change the location 
                    //of the row of the current pieces to bottom (move down) untill it meet the highest row
                    allDots[i, j] = null;
                }//why we do not build an else statement here to create pieces if it come to max j
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }
    private void ReffillBoard() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if (allDots[i, j] == null) {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }
    }
    private bool MatchesOnBoard() {         //this function is to create continuity matches when create new pieces
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {       //maybe we need to check null pieces here
                if (allDots[i, j] != null) {
                    if (allDots[i, j].GetComponent<Dot>().Matched) {//a little changes heres
                        return true;
                    }
                }
                
            }
        }
        return false;
    }
    private IEnumerator FillBoardCo() {
        ReffillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard()) {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }
}
