using GoblinGames.DesignPattern;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode
{
    AiMode,
    P2Mode,
}

public class GameManager : MonoSingleton<GameManager>
{
    private GameController gameController;
    private Board board;
    private InGameUI inGameUI;
    private GameMode gameMode;
    private int countImpediments;
    private float inGameTime;

    public GameController GameController { get { return gameController; } }
    public Board Board { get { return board; } }
    public InGameUI InGameUI { get { return inGameUI; } }
    public int CountImpediments { get { return countImpediments; } set { countImpediments = value; } }
    public float InGameTime { get {  return inGameTime; } }
    // Start is called before the first frame update

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadedsceneEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController != null)
        {
            gameController.Update();
        }

        if( board != null)
        {
            inGameTime += Time.deltaTime;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetGameMode(GameMode mode)
    {
        gameMode = mode;
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        if (sceneName.CompareTo("InGame") == 0)
        {
            board = GameObject.Find("Board").GetComponent<Board>();
            inGameUI = GameObject.Find("InGameUI").GetComponent<InGameUI>();
            inGameTime = 0f;

            switch (gameMode)
            {
                case GameMode.AiMode:
                    {
                        gameController = new AiModeController();
                        break;
                    }
                case GameMode.P2Mode:
                    {
                        gameController = new SingleModeController();
                        break;
                    }
            }

            gameController.InitGame();
        }
        else if (sceneName.CompareTo("Title") == 0)
        {
            board = null;
        }
    }
}
