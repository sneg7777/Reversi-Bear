using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button buttonAiMode;
    [SerializeField] private Button buttonP2Mode;

    private void OnEnable()
    {
        buttonAiMode.onClick.AddListener(OnClickAiMode);
        buttonP2Mode.onClick.AddListener(OnClickP2Mode);
    }

    private void OnDisable()
    {
        buttonAiMode.onClick.RemoveListener(OnClickAiMode);
        buttonP2Mode.onClick.RemoveListener(OnClickP2Mode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClickAiMode()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.LoadScene("InGame");
        gameManager.SetGameMode(GameMode.AiMode);
    }

    private void OnClickP2Mode()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.LoadScene("InGame");
        gameManager.SetGameMode(GameMode.P2Mode);
    }
}
