using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonOk;

    public Button ButtonCancel { get { return buttonClose; } }
    public Button ButtonOk { get { return buttonOk; } }

    protected virtual void OnEnable()
    {
        buttonClose.onClick.AddListener(OnClickCancel);
    }

    protected virtual void OnDisable()
    {
        buttonClose.onClick.RemoveListener(OnClickCancel);
    }

    private void OnClickCancel()
    {
        gameObject.SetActive(false);
    }

}
