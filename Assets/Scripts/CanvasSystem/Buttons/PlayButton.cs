using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CanvasSystem.Buttons
{
    public class PlayButton : MonoBehaviour, IDisposable
    {
        #region Events

        public Action PlayButtonClicked;

        #endregion Events
        
        #region Variables

        [SerializeField] private Button _button;

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
            SetupButton();
        }

        public void Dispose()
        {
            _button = null;
        }

        private void SetupButton()
        {
            ResetButton();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void ResetButton()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void OnButtonClicked()
        {
            PlayButtonClicked?.Invoke();
            ResetButton();
        }

        #endregion Functions
    }
}