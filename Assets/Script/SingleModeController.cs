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
        int score = board.PlaceStone(turn, tile.Number / Board.CountLines, tile.Number % Board.CountLines);
        if (score == -1)
        {
            return;
        }

        AddScore(turn, score);
        PassTurn();
        board.ClearForPossiblePlaced();
        if (!board.SearchForPossiblePlaced(turn))
        {
            PassTurn();
            if(!board.SearchForPossiblePlaced(turn))
            {
                GameEnd();
            }
        }
            
    }


}
