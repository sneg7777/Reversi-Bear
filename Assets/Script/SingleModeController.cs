using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleModeController : GameController
{
    // Start is called before the first frame update
    public override void InitGame()
    {
        base.InitGame();
        Board board = GameManager.Instance.Board;
        board.SearchForPossiblePlaced(turn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ClickOnTile(Tile tile)
    {
        Board board = GameManager.Instance.Board;
        if(board.PlaceStone(turn, tile.Number / Board.CountLines, tile.Number % Board.CountLines))
        {
            PassTurn();
            board.ClearForPossiblePlaced();
            if (!board.SearchForPossiblePlaced(turn))
            {
                PassTurn();
            }
        }

    }
}
