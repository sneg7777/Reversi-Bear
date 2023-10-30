using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button buttonBackTitleScene;
    [SerializeField] private Button buttonOpenSettingPopup;
    [SerializeField] private GameObject popups;
    [SerializeField] private InGameAlarm alarm;


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
            popups.transform.GetChild(i).GetComponent<Popup>().ButtonOk.onClick.RemoveAllListeners();
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
