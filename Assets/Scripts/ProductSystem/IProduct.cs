using System;

namespace Assets.Scripts.ProductSystem
{
    public interface IProduct<T> : IDisposable
    {
        #region Properties

        public Action<T> SendToPool { get; set; }

        #endregion Properties
    }
}