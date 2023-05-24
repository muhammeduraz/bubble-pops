using System;
using UnityEngine;

namespace Assets.Scripts.SubscriberSystem
{
    [DefaultExecutionOrder(-99)]
    public abstract class BaseSubscriber : MonoBehaviour, IDisposable
    {
        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            OnInitialize();
            Subscribe();
        }

        public virtual void Dispose()
        {
            UnSubscribe();
            OnDispose();
        }

        protected virtual void OnInitialize()
        {

        }

        protected virtual void OnDispose()
        {

        }

        protected abstract void Subscribe();
        protected abstract void UnSubscribe();

        #endregion Functions
    }
}