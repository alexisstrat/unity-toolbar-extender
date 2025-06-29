using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UnityToolbarExtender.Editor
{
    [CustomEditor(typeof(ToolbarExtenderConfiguration))]
    public class ToolbarExtenderConfigurationEditor : UnityEditor.Editor
    {
        private const float Seconds = 0.2f;
        private static EditorWaitForSeconds _waitForSeconds;
        
        public override VisualElement CreateInspectorGUI()
        {
            var defaultInspector = new VisualElement();
            InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
            
            var refreshButton = new Button()
            {
                text = "Manual Refresh"
            };
            refreshButton.clicked += () =>
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(DelayedClick());
            };
            
            defaultInspector.Add(refreshButton);
            return defaultInspector;
        }

        private void OnEnable()
        {
            _waitForSeconds = new EditorWaitForSeconds(Seconds);
        }

        private static IEnumerator DelayedClick()
        {
            yield return _waitForSeconds;
            ToolbarExtender.Refresh();
        }
    }
}