using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender.Editor
{
    [CreateAssetMenu(menuName = "Toolbar Extender/Scene Button")]
    public class ToolbarSceneButton : ToolbarButton
    {
        public SceneAsset sceneAsset;
        protected override void OnClick()
        {
            var activeScene = SceneManager.GetActiveScene().path;
            var newScenePath = AssetDatabase.GetAssetPath(sceneAsset);
            if (activeScene.Equals(newScenePath))
            {
                return;
            }
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Single);
            }
        }
    }
}