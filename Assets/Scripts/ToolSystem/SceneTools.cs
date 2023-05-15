using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Assets.Scripts.SceneSystem
{
    public class SceneTools
    {
        #region Variables



        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                //case PlayModeStateChange.EnteredEditMode:

                //    if (StartFromMainScene)
                //    {
                //        if (LoadLastSceneWhenExitPlayMode)
                //        {
                //            for (int i = 0; i < SceneManager.sceneCount + 1; i++)
                //            {
                //                var path = EditorPrefs.GetString("lastScene", "");
                //                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                //            }
                //        }
                //    }

                //    break;

                case PlayModeStateChange.EnteredPlayMode:
                    break;


                case PlayModeStateChange.ExitingPlayMode:
                    break;

                case PlayModeStateChange.ExitingEditMode:

                    EditorSceneManager.CloseScene(EditorSceneManager.GetActiveScene(), true);
                    
                    EditorSceneManager.SaveOpenScenes();
                    string mainScenePath = "Assets/Scenes/StartScene.unity";
                    SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(mainScenePath);

                    if (startScene != null)
                    {
                        EditorSceneManager.playModeStartScene = startScene;
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion Functions
    }
}