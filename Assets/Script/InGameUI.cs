using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button buttonBackScene;
    [SerializeField] private Button buttonOpenSettingPopup;

    private void OnEnable()
    {
        buttonBackScene.onClick.AddListener(OnClickBackScene);
        buttonOpenSettingPopup.onClick.AddListener(OnClickOpenSettingPopup);
    }

    private void OnDisable()
    {
        buttonBackScene.onClick.RemoveListener(OnClickBackScene);
        buttonOpenSettingPopup.onClick.RemoveListener(OnClickOpenSettingPopup);
    }

    private void OnClickBackScene()
    {
        GameManager.Instance.LoadScene("Title");
    }

    private void OnClickOpenSettingPopup()
    {

    }
}
