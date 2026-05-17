#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CoreBreach.Editor
{
    [InitializeOnLoad]
    public static class PlayFromIntroScene
    {
        private const string IntroScenePath = "Assets/Scenes/IntroScene.unity";

        static PlayFromIntroScene()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(IntroScenePath);
            }
        }
    }
}
#endif