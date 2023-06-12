using UnityEngine;

namespace Assets.Scripts.SubscribeSystem
{
    [DefaultExecutionOrder(-5000)]
    public abstract class BaseSubscriber : MonoBehaviour
    {
        #region Unity Functions

        private void Awake()
        {
            Initialize();
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnSubscribeEvents();
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        protected abstract void Initialize();
        protected abstract void Dispose();

        protected abstract void SubscribeEvents();
        protected abstract void UnSubscribeEvents();

        #endregion Functions
    }
}