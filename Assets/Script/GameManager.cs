using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GoblinGames.DesignPattern;

public class GameManager : MonoSingleton<GameManager>
{
    private GameController gameController;
    [SerializeField] private Board board;
    public GameController GameController { get { return gameController; } }
    public Board Board { get { return board; } }
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
