using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoblinGames.DesignPattern;
using TMPro;

public class GameManager : MonoSingleton<GameManager>
{
    private GameController gameController;
    [SerializeField] private Board board;
    [SerializeField] private GameObject showCurrentTurnImage;
    [SerializeField] private TMP_Text textPlayer1Score;
    [SerializeField] private TMP_Text textPlayer2Score;

    public GameController GameController { get { return gameController; } }
    public Board Board { get { return board; } }
    public GameObject ShowTurnImage { get { return showCurrentTurnImage; } }
    public TMP_Text TextPlayer1Score { get { return textPlayer1Score; } }
    public TMP_Text TextPlayer2Score { get { return textPlayer2Score; } }
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();

        gameController = new SingleModeController();
    }

    private void Start()
    {
        gameController.InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
