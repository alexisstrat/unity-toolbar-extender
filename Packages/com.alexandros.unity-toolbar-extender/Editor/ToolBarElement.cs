using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityToolbarExtender.Editor
{
    public abstract class ToolBarElement : ScriptableObject
    {
        internal event Action OnElementChanged;
        
        public VisualElement Create()
        {
            var element = CreateElement();
            element.style.overflow = Overflow.Visible;
            return element;
        }

        protected abstract VisualElement CreateElement();

        private void OnValidate()
        {
            OnElementChanged?.Invoke();
        }
    }
}