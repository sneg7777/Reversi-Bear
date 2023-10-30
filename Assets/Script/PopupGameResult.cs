using TMPro;
using UnityEngine;

public class PopupGameResult : Popup
{
    [SerializeField] private TMP_Text textGameTime;
    [SerializeField] private TMP_Text textPlayer1_Result;
    [SerializeField] private TMP_Text textPlayer1_Count;
    [SerializeField] private TMP_Text textPlayer1_Score;
    [SerializeField] private TMP_Text textPlayer2_Result;
    [SerializeField] private TMP_Text textPlayer2_Count;
    [SerializeField] private TMP_Text textPlayer2_Score;

    public TMP_Text TextGameTime { get { return textGameTime; } }
    public TMP_Text TextPlayer1_Result { get { return textPlayer1_Result; } }
    public TMP_Text TextPlayer1_Count { get { return textPlayer1_Count; } }
    public TMP_Text TextPlayer1_Score { get { return textPlayer1_Score; } }
    public TMP_Text TextPlayer2_Result { get { return textPlayer2_Result; } }
    public TMP_Text TextPlayer2_Count { get { return textPlayer2_Count; } }
    public TMP_Text TextPlayer2_Score { get { return textPlayer2_Score; } }

    protected override void OnClickClose()
    {
        base.OnClickClose();
        GameManager.Instance.LoadScene("Title");
    }
}