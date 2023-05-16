using System.Collections.Generic;

namespace Assets.Scripts.ProductSystem.Pool
{
    public class BasePool<T> where T : IProduct<T>
    {
        #region Variables

        private Stack<T> _stack;

        #endregion Variables

        #region Functions

        public BasePool()
        {
            _stack = new Stack<T>();
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