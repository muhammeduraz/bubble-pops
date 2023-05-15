using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.SceneSystem;

namespace Assets.Scripts.LevelSystem.Data
{
    [CreateAssetMenu (fileName = "LevelData", menuName = "Scriptable Objects/Data/Level/LevelData")]
    public class LevelData : ScriptableObject
    {
        #region Variables

        [BoxGroup("Scene")][SerializeField] private SceneReference _sceneReference;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

<<<<<<< Updated upstream
        private void Initialize()
=======
        public void Initialize()
>>>>>>> Stashed changes
        {

        }

<<<<<<< Updated upstream
        private void Terminate()
=======
        public void Dispose()
>>>>>>> Stashed changes
        {

        }

        #endregion Functions
    }
}