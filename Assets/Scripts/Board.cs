using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Board : MonoBehaviour{

    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] dots;
    private BackgroundTile[,] allTiles;
    public GameObject[,] allDots;

    void Start(){
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }
    private void SetUp()
    {
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                Vector2 temptPosition = new Vector2(i, j);
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
    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().Matched)       //if we want to change/access properties of other class, must use GetComponent()
        {
            Destroy(allDots[column, row]);
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
    }
}
