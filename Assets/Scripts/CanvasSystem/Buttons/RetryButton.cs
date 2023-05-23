using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.SceneSystem;

namespace Assets.Scripts.CanvasSystem.Buttons
{
    public class RetryButton : MonoBehaviour, IDisposable
    {
        #region Variables

        private SceneService _sceneService;

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
            _sceneService = FindObjectOfType<SceneService>();

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
            _sceneService.ReloadCurrentLevel();
        }

        #endregion Functions
    }
}