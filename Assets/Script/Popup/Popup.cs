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
        buttonClose.onClick.AddListener(OnClickClose);
    }

    protected virtual void OnDisable()
    {
        buttonClose.onClick.RemoveListener(OnClickClose);
    }

    public virtual void OnOpen()
    {

    }

    protected virtual void OnClickClose()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }


}
