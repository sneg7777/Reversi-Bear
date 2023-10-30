using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button buttonAiMode;
    [SerializeField] private Button buttonP2Mode;
    [SerializeField] private PopupSlider PopupSetImpediments;
    [SerializeField] private GameObject popups;

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

    public void OpenPopup(TitlePopupKind kind)
    {
        int popupCount = popups.transform.childCount;
        for (int i = 0; i < popupCount; i++)
        {
            popups.transform.GetChild(i).gameObject.SetActive(false);
        }
        popups.transform.GetChild((int)kind).gameObject.SetActive(true);
        popups.transform.GetChild((int)kind).GetComponent<Popup>().OnOpen();
        popups.SetActive(true);
    }

    private void OnClickAiMode()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.SetGameMode(GameMode.AiMode);
        OpenPopup(TitlePopupKind.SetImpediments);
    }

    private void OnClickP2Mode()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.SetGameMode(GameMode.P2Mode);
        OpenPopup(TitlePopupKind.SetImpediments);
    }

    private void OnClickPopupSetImpedimentsOk()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.CountImpediments = (int)PopupSetImpediments.Slider.value;
        gameManager.LoadScene("InGame");
    }
}
