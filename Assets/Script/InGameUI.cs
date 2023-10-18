using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button buttonBackScene;

    private void OnEnable()
    {
        buttonBackScene.onClick.AddListener(OnClickBackScene);
    }

    private void OnDisable()
    {
        buttonBackScene.onClick.RemoveListener(OnClickBackScene);
    }

    private void OnClickBackScene()
    {
        GameManager.Instance.LoadScene("Title");
    }
}
