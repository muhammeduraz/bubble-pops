using Assets.Scripts.ProductSystem;

namespace Assets.Scripts.FactorySystem
{
    public interface IFactory
    {
        #region Properties



        #endregion Properties

        #region Functions

        public abstract IProduct Manufacture();

        #endregion Functions
    }
}