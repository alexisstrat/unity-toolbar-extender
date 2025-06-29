using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityToolbarExtender.Editor
{
    [CreateAssetMenu(menuName = "Toolbar Extender/Configuration", order = 0)]
    internal class ToolbarExtenderConfiguration : ScriptableObject
    {
        public List<ToolbarEntry> toolbarEntries;

        private void OnValidate()
        {
            ToolbarExtender.Refresh();
        }

        private void OnEnable()
        {
            foreach (var entry in toolbarEntries)
            {
                if (!entry.element) continue;
                
                entry.element.OnElementChanged -= ToolbarExtender.Refresh;
                entry.element.OnElementChanged += ToolbarExtender.Refresh;
            }
        }
        
        private void OnDisable()
        {
            if (toolbarEntries == null) return;

            foreach (var entry in toolbarEntries)
            {
                if (!entry.element) continue;
                
                entry.element.OnElementChanged -= ToolbarExtender.Refresh;
            }
        }
    }

    [Serializable]
    internal class ToolbarEntry
    {
        public ToolBarElement element;
        public ToolbarAlign align;
    }
}