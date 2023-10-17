using UnityEngine;

public class GameController
{
    protected Team turn;
    protected int player1Score;
    protected int player2Score;
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
        GameManager gameManager =  GameManager.Instance;
        if (turn == Team.Player1)
        {
            turn = Team.Player2;
            gameManager.ShowTurnImage.transform.GetChild(0).gameObject.SetActive(false);
            gameManager.ShowTurnImage.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            turn = Team.Player1;
            gameManager.ShowTurnImage.transform.GetChild(0).gameObject.SetActive(true);
            gameManager.ShowTurnImage.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    protected void GameEnd()
    {

    }

    protected void AddScore(Team team, int score)
    {
        GameManager gameManager = GameManager.Instance;
        if (team == Team.Player1)
        {
            player1Score += score;
            gameManager.TextPlayer1Score.text = "Player : " + player1Score;
        }
        else if (team == Team.Player2)
        {
            player2Score += score;
            gameManager.TextPlayer2Score.text = "Player : " + player2Score;
        }
    }
}
