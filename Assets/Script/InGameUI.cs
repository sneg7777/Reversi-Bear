using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button buttonBackScene;
    [SerializeField] private Button buttonOpenSettingPopup;
    [SerializeField] private Popup popupBack;
    private void OnEnable()
    {
        buttonBackScene.onClick.AddListener(OnClickBackScene);
        buttonOpenSettingPopup.onClick.AddListener(OnClickOpenSettingPopup);
        popupBack.ButtonOk.onClick.AddListener(OnClickBackSceneOk);
    }

    private void OnDisable()
    {
        buttonBackScene.onClick.RemoveListener(OnClickBackScene);
        buttonOpenSettingPopup.onClick.RemoveListener(OnClickOpenSettingPopup);
        popupBack.ButtonOk.onClick.RemoveListener(OnClickBackSceneOk);
    }

    private void OnClickBackScene()
    {
        popupBack.gameObject.SetActive(true);
    }

    private void OnClickBackSceneOk()
    {
        GameManager.Instance.LoadScene("Title");
    }

    private void OnClickOpenSettingPopup()
    {

    }
}
