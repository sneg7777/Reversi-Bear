using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;


public class GameController
{
    protected Team turn;


    // Start is called before the first frame update
    public virtual void InitGame()
    {
        turn = Team.Player1;
        GameManager.Instance.Board.PlaceStone(Team.Player1, 27 / Board.CountLines, 27 % Board.CountLines);
        GameManager.Instance.Board.PlaceStone(Team.Player2, 28 / Board.CountLines, 28 % Board.CountLines);
        GameManager.Instance.Board.PlaceStone(Team.Player2, 35 / Board.CountLines, 35 % Board.CountLines);
        GameManager.Instance.Board.PlaceStone(Team.Player1, 36 / Board.CountLines, 36 % Board.CountLines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ClickOnTile(Tile tile)
    {
        
    }

    protected void PassTurn()
    {
        if(turn == Team.Player1)
        {
            turn = Team.Player2;
        }
        else
        {
            turn = Team.Player1;
        }
    }
}
