using UnityEngine;
using UnityEngine.UIElements;

namespace UnityToolbarExtender.Editor
{
    public abstract class ToolbarSlider : ToolBarElement
    {
        [SerializeField] protected string label;
        [SerializeField] protected float width = 100;
        [SerializeField] protected float minValue = -1;
        [SerializeField] protected float maxValue = 1;
        [SerializeField] protected bool showCurrentValue = true;
        [SerializeField, HideInInspector] protected float currentValue;

        protected Slider Slider;
        protected Label CurrentValueLabel;
        
        private const string Format = "F2";
        
        protected override VisualElement CreateElement()
        {
            var container = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row
                }
            };

            if (!string.IsNullOrEmpty(label))
            {
                var sliderLabel = new Label(label)
                {
                    style =
                    {
                        flexShrink = 1,
                        alignSelf = Align.Center
                    }
                };
                container.Add(sliderLabel);
            }
            
            Slider = new Slider(minValue, maxValue)
            {
                style =
                {
                    width = width,
                },
                value = currentValue
            };
            
            Slider.RegisterValueChangedCallback(OnValueChanged);
            if (showCurrentValue)
            {
                CurrentValueLabel = new Label(Slider.value.ToString(Format))
                {
                    style =
                    {
                        width = 30,
                        alignSelf = Align.Center
                    }
                };
            }
            
            container.Add(Slider);
            if (showCurrentValue)
            {
                container.Add(CurrentValueLabel);
            }
            
            return container;
        }
        
        private void OnValueChanged(ChangeEvent<float> evt)
        {
            currentValue = evt.newValue;
            if (showCurrentValue)
            {
                CurrentValueLabel.text = currentValue.ToString(Format);
            }
            OnValueChanged(evt.newValue);
        }
        
        protected abstract void OnValueChanged(float newValue);
    }
}