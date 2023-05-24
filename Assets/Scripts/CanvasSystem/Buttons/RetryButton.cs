using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasSystem.Buttons
{
    public class RetryButton : MonoBehaviour, IDisposable
    {
        #region Events

        public Action ReloadSceneRequested;

        #endregion Events,

        #region Variables

        [SerializeField] private Button _retryButton;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            InitializeButton();
        }

        public void Dispose()
        {
            DisposeButton();
        }

        private void InitializeButton()
        {
            DisposeButton();
            _retryButton.onClick.AddListener(OnRetryButtonClicked);
        }

        private void DisposeButton()
        {
            _retryButton.onClick.RemoveAllListeners();
        }

        private void OnRetryButtonClicked()
        {
            _retryButton.enabled = false;
            ReloadSceneRequested?.Invoke();
        }

        #endregion Functions
    }
}