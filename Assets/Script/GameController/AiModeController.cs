using UnityEngine;

public class AiModeController : GameController
{
    private const float delayPlacedByAi = 1f;
    private float tickPlacedByAi = 1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(isGameEnd)
        {
            return;
        }

        ProcessAi();
    }

    public override void ClickOnTile(Tile tile)
    {
        Board board = GameManager.Instance.Board;
        int score = board.PlaceStone(turn, tile.Number / Board.CountLines, tile.Number % Board.CountLines);
        if (score == -1)
        {
            return;
        }

        AddScore(turn, score);
        PassTurn();
        board.ClearForPossiblePlaced();
        board.SearchForPossiblePlaced(turn, false);
    }

    private void ProcessAi()
    {
        if (turn != Team.Player2)
        {
            return;
        }
        Board board = GameManager.Instance.Board;
        if (board.CountStoneTurningOver > 0)
        {
            return;
        }

        tickPlacedByAi -= Time.deltaTime;
        if (tickPlacedByAi > 0)
        {
            return;
        }

        tickPlacedByAi = delayPlacedByAi;

        int score = board.PlaceStoneByAi();
        PassTurn();
        board.ClearForPossiblePlaced();
        if (score == -1)
        {
            if (!board.SearchForPossiblePlaced(turn, true))
            {
                GameEnd();
            }
        }
        else
        {
            AddScore(Team.Player2, score);
            if (!board.SearchForPossiblePlaced(turn, true))
            {
                AutoPassTurn();
            }
        }
    }

}
