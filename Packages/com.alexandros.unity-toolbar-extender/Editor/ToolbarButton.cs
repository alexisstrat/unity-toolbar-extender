using UnityEngine.UIElements;

namespace UnityToolbarExtender.Editor
{
    public abstract class ToolbarButton : ToolBarElement
    {
        public string buttonText;

        protected override VisualElement CreateElement()
        {
            var button = new Button()
            {
                text = buttonText
            };
            
            button.clicked += OnClick;
            return button;
        }

        protected abstract void OnClick();
    }
}