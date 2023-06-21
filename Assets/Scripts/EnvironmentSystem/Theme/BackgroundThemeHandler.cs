using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.EnvironmentSystem.Theme.Data;

namespace Assets.Scripts.EnvironmentSystem.Theme
{
    public class BackgroundThemeHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private Tween _themeTween;

        [SerializeField] private float _colorChangeDuration;

        [SerializeField] private ThemeDataSO _themeDataSO;
        [SerializeField] private SpriteRenderer _backgroundSprite;
        [SerializeField] private SpriteRenderer _backgroundPlaneRenderer;

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
            _themeDataSO = null;
            _backgroundSprite = null;
        }

        public void SetBackgroundSpriteColor()
        {
            if (_themeDataSO.CurrentThemeData == null) return;

            _themeTween?.Kill();
            _themeTween = _backgroundSprite.DOColor(_themeDataSO.CurrentThemeData.BackgroundColor, _colorChangeDuration);

            _backgroundPlaneRenderer.color = _themeDataSO.CurrentThemeData.BackgroundColor;
        }

        #endregion Functions
    }
}