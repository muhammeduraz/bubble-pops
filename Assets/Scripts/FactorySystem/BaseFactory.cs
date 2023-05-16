using Assets.Scripts.ProductSystem;

namespace Assets.Scripts.FactorySystem
{
    public abstract class BaseFactory<T> where T : IProduct<T>
    {
        #region Functions

        public BaseFactory()
        {

        }

        public abstract T Manufacture();

        #endregion Functions
    }
}