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
    [SerializeField] private Board board;
    private GameMode gameMode;

    public GameController GameController { get { return gameController; } }
    public Board Board { get { return board; } }
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
