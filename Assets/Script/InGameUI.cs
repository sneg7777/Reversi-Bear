using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button buttonBackTitleScene;
    [SerializeField] private Button buttonOpenSettingPopup;
    [SerializeField] private GameObject popups;
    [SerializeField] private InGameAlarm alarm;
    [SerializeField] private PopupGameResult popupGameResult;

    private void OnEnable()
    {
        buttonBackTitleScene.onClick.AddListener(OnClickBackTitleScene);
        buttonOpenSettingPopup.onClick.AddListener(OnClickOpenSettingPopup);
        popups.transform.GetChild((int)InGamePopupKind.BackTitleScene).GetComponent<Popup>().ButtonOk.onClick.AddListener(OnClickBackTitleSceneOk);
    }

    private void OnDisable()
    {
        buttonBackTitleScene.onClick.RemoveListener(OnClickBackTitleScene);
        buttonOpenSettingPopup.onClick.RemoveListener(OnClickOpenSettingPopup);
        int popupCount = popups.transform.childCount;
        for (int i = 0; i < popupCount; i++)
        {
            popups.transform.GetChild(i).GetComponent<Popup>().ButtonOk?.onClick.RemoveAllListeners();
        }
    }

    public void SetAlarm(AlarmKind kind)
    {
        alarm.SetAlarm(kind);
    }

    public void OpenPopup(InGamePopupKind kind)
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

    public void ClosePopup(InGamePopupKind kind)
    {
        popups.transform.GetChild((int)kind).gameObject.SetActive(false);
        popups.SetActive(false);
    }

    public void SetPopupGameResult(int player1Count, int player1Score, int player2Count, int player2Score)
    {
        float time = GameManager.Instance.InGameTime;
        popupGameResult.TextGameTime.text = "Time : " + Mathf.FloorToInt(time / 60f) + ": " + Mathf.RoundToInt(time % 60f);

        popupGameResult.TextPlayer1_Count.text = "Count : " + player1Count;
        popupGameResult.TextPlayer1_Score.text = "Score : " + player1Score;
        popupGameResult.TextPlayer2_Count.text = "Count : " + player2Count;
        popupGameResult.TextPlayer2_Score.text = "Score : " + player2Score;

        if(player1Count > player2Count)
        {
            popupGameResult.TextPlayer1_Result.text = "Win !";
            popupGameResult.TextPlayer2_Result.text = "Lose..";
        }
        else
        {
            popupGameResult.TextPlayer1_Result.text = "Lose..";
            popupGameResult.TextPlayer2_Result.text = "Win !";
        }
    }

    private void OnClickBackTitleScene()
    {
        OpenPopup(InGamePopupKind.BackTitleScene);
    }

    private void OnClickBackTitleSceneOk()
    {
        GameManager.Instance.LoadScene("Title");
    }

    private void OnClickOpenSettingPopup()
    {

    }
}
