using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.Creators
{
    public class ScriptableCreator
    {
        #region Functions

        public void CreateInstance<T>(string name, string path) where T : ScriptableObject
        {
            T scriptable = default;
            string assetPath = Path.Combine(path, $"{name}.asset");

            scriptable = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(scriptable, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion Functions
    }
}