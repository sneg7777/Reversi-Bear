public class SingleModeController : GameController
{
    public override void InitGame()
    {
        base.InitGame();
    }

    public override void Update()
    {
        base.Update();
        if (isGameEnd)
        {
            return;
        }

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
        if (!board.SearchForPossiblePlaced(turn, true))
        {
            AutoPassTurn();
            if (!board.SearchForPossiblePlaced(turn, true))
            {
                GameEnd();
            }
        }

    }


}
