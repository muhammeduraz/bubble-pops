using System;
using System.Collections.Generic;
using Assets.Scripts.FactorySystem;

namespace Assets.Scripts.ProductSystem.Pool
{
    public class GenericPool<T> : IDisposable
    {
        #region Variables

        private GenericFactory<T> _genericFactory;

        private Stack<T> _stack;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public GenericPool()
        {
            
        }

        public void Initialize()
        {
            _stack = new Stack<T>();
        }

        public void Dispose()
        {
            _stack = null;
        }

        private void OnItemSendToPool(T product)
        {
            //product.OnProductSendToPool -= OnItemSendToPool;
            _stack.Push(product);
        }

        private T GetProductFromPool()
        {
            _stack.TryPop(out T product);

            return product;
        }

        private T GetProductFromFactory()
        {
            return _genericFactory.Manufacture();
        }

        public T GetProduct()
        {
            T product = default;

            product = GetProductFromPool();
            if (product != null)
            {
                //product.OnProductSendToPool += OnItemSendToPool;
                return product;
            }

            product = GetProductFromFactory();
            if (product != null)
            {
                //product.OnProductSendToPool += OnItemSendToPool;
                return product;
            }

            return default;
        }

        #endregion Functions
    }
}