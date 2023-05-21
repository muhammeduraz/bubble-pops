using System.Collections.Generic;
using Assets.Scripts.FactorySystem;

namespace Assets.Scripts.ProductSystem.Pool
{
    public class BasePool<T> where T : IProduct<T>
    {
        #region Variables

        private Stack<T> _stack;
        private BaseFactory<T> _factory;

        #endregion Variables

        #region Functions

        public BasePool(BaseFactory<T> factory)
        {
            _stack = new Stack<T>();
            _factory = factory;
        }

        ~BasePool()
        {
            _stack = null;
        }

        private void OnItemSendToPool(T product)
        {
            product.SendToPool -= OnItemSendToPool;
            _stack.Push(product);
        }

        private T GetProductFromPool()
        {
            _stack.TryPop(out T product);
            return product;
        }

        public T GetProduct()
        {
            T product = GetProductFromPool();

            product ??= _factory.Manufacture();

            if (product != null)
            {
                product.SendToPool += OnItemSendToPool;
                return product;
            }

            return default;
        }

        #endregion Functions
    }
}