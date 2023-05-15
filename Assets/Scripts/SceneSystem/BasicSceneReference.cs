using System;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.SceneSystem
{
    [Serializable]
    public class BasicSceneReference
    {
        #region Variables

#if UNITY_EDITOR
        [SerializeField] private SceneAsset _sceneAsset;
#endif

        [ReadOnly][SerializeField] private string _scenePath = string.Empty;

        #endregion Variables

        #region Properties

        public string ScenePath { get => _scenePath; }

        #endregion Properties

        #region EditorFunctions

        [Button]
        private void SerializeScenePath()
        {
            _scenePath = AssetDatabase.GetAssetPath(_sceneAsset);
            AssetDatabase.SaveAssets();
        }

        #endregion EditorFunctions
    }
}