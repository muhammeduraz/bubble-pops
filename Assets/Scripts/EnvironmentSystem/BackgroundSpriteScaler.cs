using System;
using UnityEngine;

namespace Assets.Scripts.EnvironmentSystem
{
    public class BackgroundSpriteScaler : MonoBehaviour, IDisposable
    {
        #region Variables

        private Camera _camera;

        [SerializeField] private Vector2 _foregroundScaleFactor;

        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;
        [SerializeField] private SpriteRenderer _foregroundSpriteRenderer;

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
            _camera = Camera.main;

            UpdateBackgroundSprite();
        }

        public void Dispose()
        {

        }

        private void UpdateBackgroundSprite()
        {
            _backgroundSpriteRenderer.transform.localScale = GetScreenToWorldSize();
            _foregroundSpriteRenderer.transform.localScale = _foregroundScaleFactor;
        }

        private Vector2 GetScreenToWorldSize()
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            
            Vector2 size = edgeVector * 2;
            return size;
        }

        #endregion Functions
    }
}