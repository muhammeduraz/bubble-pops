using System;
using UnityEngine;

namespace Assets.Scripts.EnvironmentSystem
{
    public class BackgroundSpriteScaler : MonoBehaviour, IDisposable
    {
        #region Variables

        private Camera _camera;

        [SerializeField] private SpriteRenderer _backgroundSpriteRenderer;

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
            _camera = null;
            _backgroundSpriteRenderer = null;
        }

        private void UpdateBackgroundSprite()
        {
            _backgroundSpriteRenderer.transform.localScale = GetScreenToWorldSize();
        }

        private Vector2 GetScreenToWorldSize()
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = _camera.ViewportToWorldPoint(topRightCorner);
            
            Vector2 size = edgeVector * 2;
            return size;
        }

        #endregion Functions
    }
}