using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public enum GameState {
    wait,
    move,
    win,
    lose,
    pause
}
public enum TileKind {
    Breakable,
    Blank,
    Normal
}
[System.Serializable]   //this help unity to known the below should serilize
public class TypeTile {
    public int x;
    public int y;
    public TileKind tileKind;
}
public class Board : MonoBehaviour{

    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject[] dots;
    public GameObject destroyEffect;
    public TypeTile[] boardLayout;  //make public so it can be seen it the inspector
    private bool[,] blankSpaces;
    private BackgroundTile[,] breakableTiles;
    public GameObject[,] allDots;
    public Dot currentDot;
    private FindMatches findMatches;
    private SoundManager soundManager;
    private GoalManager goalManager;

    [Header("Scriptble Object stuff")]
    public World world;
    public int level;

    public int basePieceValue = 20;
    private int streakValue = 1;
    private ScoreManager scoreManager;
    public float refillDelay = .5f;
    public int[] scoreGoals;

    private void Awake() {
        if(PlayerPrefs.HasKey("Current Level")) {
            level = PlayerPrefs.GetInt("Current Level");
        }
        if(level < world.levels.Length) {
            if (world != null) {
                if (world.levels[level] != null) {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    dots = world.levels[level].dots;
                    scoreGoals = world.levels[level].scoreGoals;
                    boardLayout = world.levels[level].boardLayout;
                }
            }
        }
        
    }

    void Start(){
        findMatches = FindObjectOfType<FindMatches>();
        scoreManager = FindObjectOfType<ScoreManager>();
        soundManager = FindObjectOfType<SoundManager>();
        goalManager = FindObjectOfType<GoalManager>();
        blankSpaces = new bool[width, height];
        breakableTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
        currentState = GameState.pause;
    }
    public void GenerateBlankSpaces() {
        for(int i = 0; i< boardLayout.Length; i++) {
            if (boardLayout[i].tileKind == TileKind.Blank) {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }
    public void GenerateBreakableTiles() {
        for(int i = 0; i< boardLayout.Length; i++) {
            if (boardLayout[i].tileKind == TileKind.Breakable) {
                Vector2 temp = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, temp, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
                Debug.Log("Hit Points: " + breakableTiles[boardLayout[i].x, boardLayout[i].y].hitPoints);
            }
        }
    }
    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++) {
                if (!blankSpaces[i, j]) {
                    Vector2 temptPosition = new Vector2(i, j + offSet);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "( " + i + ", " + j + " )";

                    int dotToUse = Random.Range(0, dots.Length);
                    int maxInteration = 0;
                    while (MatchesAt(i, j, dots[dotToUse]) && maxInteration < 100) //why we need maxInteration ?
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
    }
    private bool MatchesAt(int column, int row, GameObject piece)       //could we have only one paramether piece ?
    {                                                                   //why piece.column is not ok
        
        if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null) {

                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) return true;
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null) {

                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) return true;
            }
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null) {

                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag) return true;
                }
            }
            if(column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null) {

                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag) return true;
                }
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
    private bool ColumnOrRow() {//this function is to check if this belong to two kind of five match (vertical or horizontial) => true
        int numberHorizotial = 0;
        int numberVertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece != null) {
            foreach(GameObject currentPiece in findMatches.currentMatches) {
                Dot dot = currentPiece.GetComponent<Dot>();
                if(dot.row == firstPiece.row) {
                    numberHorizotial++;
                }
                if(dot.column == firstPiece.column) {
                    numberVertical++;
                }
            }
        }
        Debug.Log("Column: " + numberHorizotial + "  Row: " + numberVertical);
        return (numberVertical == 5 || numberHorizotial == 5);
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
                    else {//for the case that we use the other dot to match
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

            //check if the tile need to break
            if (breakableTiles[column, row] != null) {
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0) {
                    breakableTiles[column, row] = null;
                }
            }

            if(goalManager != null) {
                goalManager.CompareGoal(allDots[column, row].tag.ToString());
                goalManager.UpdateGoal();
            }

            findMatches.currentMatches.Remove(allDots[column, row]);     //each time we destroy the matches, also remove from the list



            if(soundManager != null) {
                soundManager.PlayRandomDestroyNoise();
            }
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);


            Destroy(particle, .5f); 
            Destroy(allDots[column, row]);          //this code is to destroy the game object 
            scoreManager.IncreaseScore(basePieceValue * streakValue);
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
        StartCoroutine(DecreaseRowCo2());
    }
    private IEnumerator DecreaseRowCo2() {
        for(int i = 0; i< width; i++) {
            for(int j = 0; j < height; j++) {
                if (!blankSpaces[i, j] && allDots[i, j] == null) {
                    for(int k = j + 1; k < height; k++) {
                        if (allDots[i, k] != null) {        //shift the pieces that is not blank and not empty to the destroyed one
                            allDots[i, k].GetComponent<Dot>().row = j;
                            allDots[i, k] = null;
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refillDelay * .5f);
        StartCoroutine(FillBoardCo());
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
                if (allDots[i, j] == null && !blankSpaces[i, j]) {// && !blankSpaces[i, j]
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);//dont know. I think that should be a feature not a bug
                    int maxInteration = 0;
                    while(MatchesAt(i, j, dots[dotToUse]) && maxInteration < 100) {
                        maxInteration++;
                        dotToUse = Random.Range(0, dots.Length);
                    }
                    maxInteration = 0;
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
        yield return new WaitForSeconds(refillDelay);

        while (MatchesOnBoard()) {
            streakValue += 1;
            DestroyMatches();
            yield return new WaitForSeconds(2 * refillDelay);
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(refillDelay);
        if (DeadBlock()) {
            ShuffleBoard();
        }
        currentState = GameState.move;
        streakValue = 1;        //mean that if you destroy in a row the point would multiply
    }
    private void SwitchPieces(int column, int row, Vector2 direction) {
        //Hold the second dot
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
        //switch the first dot to be the second dot
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
        //set the first dot to be the second dot
        allDots[column, row] = holder;
    }
    private bool CheckForMatches() {//I think that we could improve by only checking the nearby dots
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if(i < width - 2) {
                        //check the dot to the right exists
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null) {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag
                                && allDots[i + 2, j].tag == allDots[i, j].tag) {
                                return true;
                            }
                        }
                    }
                    if(j < height - 2) {
                        //check dot up 
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null) {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag
                                && allDots[i, j + 2].tag == allDots[i, j].tag) {
                                return true;
                            }
                        }
                    }
                    
                }
            }
        }
        return false;
    }
    public bool SwitchAndCheck(int column, int row, Vector2 direction) {
        SwitchPieces(column, row, direction);
        if (CheckForMatches()) {
            SwitchPieces(column, row, direction);
            return true;
        }
        else//why they do not let it in the else statement
        {
            SwitchPieces(column, row, direction);
            return false;
        }
    }
    private bool DeadBlock() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if(i < width - 1) {
                        if (SwitchAndCheck(i, j, Vector2.right)) 
                            return false;
                    }
                    if(j < height - 1) {
                        if(SwitchAndCheck(i, j, Vector2.up)) 
                            return false;
                    }
                }
            }
        }
        return true;
    }
    private void ShuffleBoard() {
        List<GameObject> newBoard = new List<GameObject>();
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allDots[i, j] != null) {
                    if (!blankSpaces[i, j]) {
                        int piecesToUse = Random.Range(0, newBoard.Count);

                        int maxInteration = 0;
                        while (MatchesAt(i, j, newBoard[piecesToUse]) && maxInteration < 100) //why we need maxInteration ?
                        {
                            piecesToUse = Random.Range(0, newBoard.Count);
                            maxInteration++;
                        }
                        Dot piece = newBoard[piecesToUse].GetComponent<Dot>();      //the order of the statement is important

                        maxInteration = 0;


                        piece.column = i;
                        piece.row = j;
                        allDots[i, j] = newBoard[piecesToUse];
                        newBoard.Remove(newBoard[piecesToUse]);
                    }
                }
            }
        }

        //check if it is deadblock
        if (DeadBlock()) {
            ShuffleBoard();
        }
    }
}
