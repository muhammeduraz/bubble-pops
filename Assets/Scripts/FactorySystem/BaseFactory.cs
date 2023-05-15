using UnityEngine;
using Assets.Scripts.ProductSystem;

namespace Assets.Scripts.FactorySystem
{
    public abstract class BaseFactory<T>
    {
        #region Variables



        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public BaseFactory()
        {

        }

        public abstract T Manufacture();

        #endregion Functions
    }
}