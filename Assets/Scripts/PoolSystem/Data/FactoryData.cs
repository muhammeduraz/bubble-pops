using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.BulletSystem;

namespace Assets.Scripts.BulletSystem.Data
{
    [CreateAssetMenu (fileName = "FactoryData", menuName = "Scriptable Objects/Factory/Data/FactoryData")]
    public class FactoryData<T> : ScriptableObject
    {
        #region Variables

        [BoxGroup("Data")][SerializeField] public T _prefab;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        private void Initialize()
        {

        }

        private void Terminate()
        {

        }

        #endregion Functions
    }
}