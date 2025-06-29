using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityToolbarExtender.Editor;

[CreateAssetMenu(menuName = "Toolbar Extender/Time Scale Slider")]
public class TimeScaleToolbarElement : ToolbarSlider
{
    private const float DefaultTimeScale = 1;
    private const string Format = "F2";

    protected override VisualElement CreateElement()
    {
        var container = base.CreateElement();
        var resetButton = new Button()
        {
            text = "Reset",
            style =
            {
                overflow = Overflow.Visible,
                width = 50
            }
        };

        resetButton.clicked += ResetTime;
        container.Add(resetButton);

        if (!Application.isPlaying) return container;

        EditorApplication.update -= SyncSlider;
        EditorApplication.update += SyncSlider;
        return container;
    }

    private void ResetTime()
    {
        currentValue = DefaultTimeScale;
        Time.timeScale = currentValue;
        Slider.SetValueWithoutNotify(currentValue);
        if (showCurrentValue)
        {
            CurrentValueLabel.text = currentValue.ToString(Format);
        }
    }

    protected override void OnValueChanged(ChangeEvent<float> evt)
    {
        base.OnValueChanged(evt);
        Time.timeScale = currentValue;
    }

    private void SyncSlider()
    {
        if (Mathf.Approximately(currentValue, Time.timeScale)) return;

        currentValue = Time.timeScale;
        Slider.SetValueWithoutNotify(currentValue);
        if (showCurrentValue)
        {
            CurrentValueLabel.text = currentValue.ToString(Format);
        }
    }

    private void OnDisable()
    {
        EditorApplication.update -= SyncSlider;
    }

    private void OnDestroy()
    {
        EditorApplication.update -= SyncSlider;
    }
}


