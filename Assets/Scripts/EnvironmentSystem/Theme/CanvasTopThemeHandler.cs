using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.EnvironmentSystem.Theme.Data;

namespace Assets.Scripts.EnvironmentSystem.Theme
{
    public class CanvasTopThemeHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        [SerializeField] private Image _topImage;
        [SerializeField] private ThemeDataSO _themeDataSO;

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
            SetBackgroundSpriteColor();
        }

        public void Dispose()
        {
            _topImage = null;
            _themeDataSO = null;
        }

        private void SetBackgroundSpriteColor()
        {
            if (_themeDataSO.CurrentThemeData == null) return;

            _topImage.color = _themeDataSO.CurrentThemeData.BackgroundColor;
        }

        #endregion Functions
    }
}