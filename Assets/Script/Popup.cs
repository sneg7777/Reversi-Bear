using UnityEngine;
using UnityEngine.UI;

public enum InGamePopupKind
{
    BackTitleScene,
    GameResult,
}

public enum TitlePopupKind
{
    SetImpediments,
}


public class Popup : MonoBehaviour
{
    [SerializeField] protected Button buttonClose;
    [SerializeField] protected Button buttonOk;

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

    public virtual void OnOpen()
    {

    }

    private void OnClickCancel()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }


}
