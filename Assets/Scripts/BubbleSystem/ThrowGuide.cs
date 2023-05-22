using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.BubbleSystem
{
    public class ThrowGuide : MonoBehaviour, IDisposable
    {
        #region Variables

        private Tween _scaleTween;

        [SerializeField] private SpriteRenderer _sprite;

        [SerializeField] private Ease _scaleEase;
        [SerializeField] private float _scaleDuration;

        [SerializeField] private float _imageAlpha;

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
            transform.localScale = Vector3.zero;
        }

        public void Dispose()
        {
            _sprite = null;
        }

        public void Reset()
        {
            _scaleTween?.Kill();

            transform.localScale = Vector3.zero;
        }

        public void SetColor(Color color)
        {
            color.a = _imageAlpha;
            _sprite.color = color;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ScaleOut()
        {
            transform.localScale = Vector3.zero;

            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(1f, _scaleDuration).SetEase(_scaleEase);
        }

        #endregion Functions
    }
}