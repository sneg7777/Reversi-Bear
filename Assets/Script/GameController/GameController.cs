using UnityEngine;

public class GameController
{
    protected const float ShowResultDelay = 2f;

    protected Team turn;
    protected int player1Score;
    protected int player2Score;
    protected bool isGameEnd;
    protected float showResultTick;
    protected bool isShowResult;


    

    // Start is called before the first frame update
    public virtual void InitGame()
    {
        turn = Team.Player1;
        GameManager.Instance.Board.PlaceStone(Team.Player1, 27 / Board.CountLines, 27 % Board.CountLines);
        GameManager.Instance.Board.PlaceStone(Team.Player2, 28 / Board.CountLines, 28 % Board.CountLines);
        GameManager.Instance.Board.PlaceStone(Team.Player2, 35 / Board.CountLines, 35 % Board.CountLines);
        GameManager.Instance.Board.PlaceStone(Team.Player1, 36 / Board.CountLines, 36 % Board.CountLines);

        Board board = GameManager.Instance.Board;
        board.SettingPlacedImpediments(GameManager.Instance.CountImpediments);
        board.SearchForPossiblePlaced(turn, true);
    }

    public virtual void Update()
    {
        ProcessGameEnd();
    }

    protected void ProcessGameEnd()
    {
        if (isGameEnd)
        {
            showResultTick -= Time.deltaTime;
            if (showResultTick < 0f && !isShowResult)
            {
                isShowResult = true;
                SetGameResult();
            }
        }
    }

    public virtual void ClickOnTile(Tile tile)
    {

    }

    protected void PassTurn()
    {
        GameManager gameManager = GameManager.Instance;
        if (turn == Team.Player1)
        {
            turn = Team.Player2;
            gameManager.Board.ShowTurnImage.transform.GetChild(0).gameObject.SetActive(false);
            gameManager.Board.ShowTurnImage.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            turn = Team.Player1;
            gameManager.Board.ShowTurnImage.transform.GetChild(0).gameObject.SetActive(true);
            gameManager.Board.ShowTurnImage.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    protected void AutoPassTurn()
    {
        GameManager.Instance.InGameUI.SetAlarm(AlarmKind.AutoPassTurn);
        PassTurn();
    }

    protected void GameEnd()
    {
        isGameEnd = true;
        showResultTick = ShowResultDelay;
        GameManager.Instance.InGameUI.SetAlarm(AlarmKind.GameEnd);
    }

    protected void AddScore(Team team, int score)
    {
        GameManager gameManager = GameManager.Instance;
        if (team == Team.Player1)
        {
            player1Score += score;
            gameManager.Board.TextPlayer1Score.text = "Player : " + player1Score;
        }
        else if (team == Team.Player2)
        {
            player2Score += score;
            gameManager.Board.TextPlayer2Score.text = "Player : " + player2Score;
        }
    }

    protected void SetGameResult()
    {
        Board board = GameManager.Instance.Board;
        GameManager.Instance.InGameUI.SetPopupGameResult(board.CountPlayer1Stone, player1Score, board.CountPlayer2Stone, player2Score);
        GameManager.Instance.InGameUI.OpenPopup(InGamePopupKind.GameResult);
    }
}
