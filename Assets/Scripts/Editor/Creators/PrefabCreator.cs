using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Editor.Creators
{
    public class PrefabCreator
    {
        #region Variables

        private const string VariantPrefabCouldNotFoundMessage = "Variant prefab could not found at given path!";

        #endregion Variables

        #region Functions

        /// <summary>
        /// Creates a new prefab and saves it at given path.
        /// </summary>
        /// <param name="prefabPath"></param>
        /// <param name="staysInScene">If true, spawns created prefab into current scene.</param>
        public void CreatePrefab(string prefabName, string prefabPath, bool staysInScene = false)
        {
            string path = Path.Combine(prefabPath, $"{prefabName}.prefab");
            GameObject objectToBeprefabed = new GameObject(prefabName);
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(objectToBeprefabed, prefabPath);

            if (staysInScene == true)
            {
                PrefabUtility.InstantiatePrefab(prefab, null);
            }
        }

        /// <summary>
        /// Creates a variant prefab and saves it at given path.
        /// </summary>
        /// <param name="basePrefabPath">Base prefab path including its name and extension.</param>
        /// <param name="variantPath">Variant prefab path including its name and extension.</param>
        /// <param name="staysInScene">If true, spawns created prefab into current scene.</param>
        /// <exception cref="NullReferenceException"></exception>
        public void CreatePrefabVariant(string basePrefabPath, string variantPath, bool staysInScene = false)
        {
            Object basePrefabObject = null;
            basePrefabObject = AssetDatabase.LoadAssetAtPath<Object>(basePrefabPath);

            if (basePrefabObject == null)
            {
                throw new NullReferenceException(VariantPrefabCouldNotFoundMessage);
            }

            GameObject instantiateBasePrefab = null;
            instantiateBasePrefab = PrefabUtility.InstantiatePrefab(basePrefabObject) as GameObject;

            GameObject savedVariantPrefab = null;
            savedVariantPrefab = PrefabUtility.SaveAsPrefabAsset(instantiateBasePrefab, variantPath);

            GameObject.DestroyImmediate(instantiateBasePrefab);

            if (staysInScene == true)
            {
                PrefabUtility.InstantiatePrefab(savedVariantPrefab, null);
            }

            //PrefabUtility.ApplyPrefabInstance(instantiatedVariantPrefab, InteractionMode.AutomatedAction);
        }

        #endregion Functions
    }
}