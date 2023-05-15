using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Pool
{
    public class ObjectPool<T> where T : ObjectPool<T>.Item, new()
    {
        #region Variables

        Stack<T> m_pool = null;
        int m_instances = 0; // For debugging

        #endregion Variables

        #region Functions

        public ObjectPool(int initialCapacity, int numInitialInstances = 0)
        {
            m_pool = new Stack<T>(Mathf.Max(initialCapacity, numInitialInstances));

            for (int i = 0; i < numInitialInstances; ++i)
            {
                m_pool.Push(new T());
                ++m_instances;
            }
        }

        public T New()
        {
            T obj = null;

            if (m_pool.Count > 0)
            {
                obj = m_pool.Pop();
            }
            else
            {
                obj = new T();
                ++m_instances;
            }

            obj.ActivatedFromPool(this);
            return obj;
        }

        public void Dispose(T obj)
        {
            m_pool.Push(obj);
        }

        #endregion Functions

        #region Inner Classes

        public class Item
        {
            ObjectPool<T> m_pool = null;

            public void ActivatedFromPool(ObjectPool<T> pool)
            {
                m_pool = pool;
            }

            public void Dispose()
            {
                m_pool.Dispose((T)this);
            }
        }

        #endregion Inner Classes
    }
}