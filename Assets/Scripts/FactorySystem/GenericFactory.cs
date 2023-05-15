using Assets.Scripts.ProductSystem;

namespace Assets.Scripts.FactorySystem
{
    public class GenericFactory<T> : BaseFactory<T>
    {
        #region Variables



        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public GenericFactory() : base()
        {

        }

        public void Dispose()
        {

        }

        public override T Manufacture()
        {
            return default;
        }

        #endregion Functions
    }
}