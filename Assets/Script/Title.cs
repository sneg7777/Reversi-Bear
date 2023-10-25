using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button buttonAiMode;
    [SerializeField] private Button buttonP2Mode;
    [SerializeField] private PopupSlider PopupSetImpediments;

    private void OnEnable()
    {
        buttonAiMode.onClick.AddListener(OnClickAiMode);
        buttonP2Mode.onClick.AddListener(OnClickP2Mode);
        PopupSetImpediments.ButtonOk.onClick.AddListener(OnClickPopupSetImpedimentsOk);
    }

    private void OnDisable()
    {
        buttonAiMode.onClick.RemoveListener(OnClickAiMode);
        buttonP2Mode.onClick.RemoveListener(OnClickP2Mode);
        PopupSetImpediments.ButtonOk.onClick.RemoveListener(OnClickPopupSetImpedimentsOk);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnClickAiMode()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.SetGameMode(GameMode.AiMode);
        PopupSetImpediments.gameObject.SetActive(true);
    }

    private void OnClickP2Mode()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.SetGameMode(GameMode.P2Mode);
        PopupSetImpediments.gameObject.SetActive(true);
    }

    private void OnClickPopupSetImpedimentsOk()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.CountImpediments = (int)PopupSetImpediments.Slider.value;
        gameManager.LoadScene("InGame");
    }
}
