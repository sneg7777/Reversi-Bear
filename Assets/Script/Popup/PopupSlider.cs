using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSlider : Popup
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text textSliderValue;

    public Slider Slider { get { return slider; } }
    public TMP_Text TextSliderValue { get { return textSliderValue; } }

    protected override void OnEnable()
    {
        base.OnEnable();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        textSliderValue.text = (int)value + " / 10";
    }
}
