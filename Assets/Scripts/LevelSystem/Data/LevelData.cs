using UnityEngine;
using Assets.Scripts.SceneSystem;

namespace Assets.Scripts.LevelSystem.Data
{
    [CreateAssetMenu (fileName = "LevelData", menuName = "Scriptable Objects/Data/Level/LevelData")]
    public class LevelData : ScriptableObject
    {
        #region Variables

        [SerializeField] private SceneReference _sceneReference;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public void Initialize()
        {

        }

        public void Dispose()
        {

        }

        #endregion Functions
    }
}