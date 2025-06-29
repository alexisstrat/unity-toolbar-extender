using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityToolbarExtender.Editor
{
    public static class ToolbarExtender
    {
        private static ScriptableObject _unityEditorToolbar;
        private const string ToolbarTypeName = "UnityEditor.Toolbar";
        private const string RootField = "m_Root";
        private const string ToolbarZoneLeftAlign = "ToolbarZoneLeftAlign";
        private const string ToolbarZoneRightAlign = "ToolbarZoneRightAlign";
        private static readonly List<VisualElement> ScrollContainers = new List<VisualElement>();

        public static void Refresh()
        {
            OnRecompile();
        }

        [UnityEditor.Callbacks.DidReloadScripts(int.MaxValue)]
        private static void OnRecompile()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall -= OnRecompile;
                EditorApplication.delayCall += OnRecompile;
                return;
            }
            EditorApplication.delayCall -= OnRecompile;

            for (var index = ScrollContainers.Count - 1; index >= 0; index--)
            {
                var element = ScrollContainers[index];
                element.RemoveFromHierarchy();
            }

            ScrollContainers.Clear();
            
            var configurations = GatherConfigurations();
            if (configurations.Count == 0)
            {
                return;
            }
            
            if (!_unityEditorToolbar)
            {
                _unityEditorToolbar = GetToolbar();
            }

            var rootElement = GetRoot();
            if (rootElement == null)
            {
                Debug.LogWarning("Root element for Toolbar not found");
                return;
            }

            var leftZone = rootElement.Q<VisualElement>(ToolbarZoneLeftAlign);
            var rightZone = rootElement.Q<VisualElement>(ToolbarZoneRightAlign);

            var leftContainer = CreateScrollView();
            leftContainer.Q<VisualElement>("unity-content-container").style.justifyContent = Justify.FlexEnd;

            var rightContainer = CreateScrollView();

            leftZone.Add(leftContainer);
            rightZone.Add(rightContainer);
            ScrollContainers.Add(leftContainer);
            ScrollContainers.Add(rightContainer);
            
            foreach (var configuration in configurations)
            {
                foreach (var toolbarEntry in configuration.toolbarEntries)
                {
                    if (!toolbarEntry.element) continue;
                    
                    switch (toolbarEntry.align)
                    {
                        case ToolbarAlign.Right:
                            rightContainer.Add(toolbarEntry.element.Create());
                            break;
                        case ToolbarAlign.Left:
                        default:
                            leftContainer.Add(toolbarEntry.element.Create());
                            break;
                    }
                }
            }
        }
        
        private static List<ToolbarExtenderConfiguration> GatherConfigurations()
        {
            var assets = AssetDatabase.FindAssets($"t: {nameof(ToolbarExtenderConfiguration)}");
            var configurations = new List<ToolbarExtenderConfiguration>();
            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var configuration = AssetDatabase.LoadAssetAtPath<ToolbarExtenderConfiguration>(path);
                configurations.Add(configuration);
            }

            return configurations;
        }

        private static ScriptableObject GetToolbar()
        {
            var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType(ToolbarTypeName);
            var toolBars = Resources.FindObjectsOfTypeAll(toolbarType);
            if (toolBars.Length > 0)
            {
                return toolBars[0] as ScriptableObject;
            }

            return null;
        }

        private static VisualElement GetRoot()
        {
            if (!_unityEditorToolbar)
            {
                _unityEditorToolbar = GetToolbar();
            }
            
            var toolbarType = _unityEditorToolbar.GetType();
            var field = toolbarType
                .GetField(RootField, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return field.GetValue(_unityEditorToolbar) as VisualElement;
            }

            return null;
        }

        private static ScrollView CreateScrollView()
        {
            var scrollView = new ScrollView(ScrollViewMode.Horizontal)
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    flexShrink = 1,
                    flexGrow = 1,
                    position = Position.Relative,
                },
                verticalScrollerVisibility = ScrollerVisibility.Hidden
            };

            var scroller = scrollView.horizontalScroller;
            var leftButton = scroller.lowButton;
            var rightButton = scroller.highButton;
            var slider = scroller.slider;

            scroller.style.height = 1;
            scroller.style.borderTopWidth = 0;
            leftButton.style.height = rightButton.style.height = slider.style.height = 5;
            
            return scrollView;
        }
    }
}