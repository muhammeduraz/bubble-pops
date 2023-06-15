using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.BubbleSystem.MenuSceneBubbles
{
    public class SlideBubble : MonoBehaviour, IDisposable
    {
        #region Events

        public Action<SlideBubble> Terminated;

        #endregion Events
        
        #region Variables

        private Tween _movementTween;

        [SerializeField] private TextMeshPro _idText;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion Variables

        #region Unity Functions

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        public void Initialize(Vector3 startPosition, Vector3 scale, float movementDuration, float targetHeight, BubbleData bubbleData)
        {
            _idText.text = "" + bubbleData.id;
            _spriteRenderer.color = new Color(bubbleData.color.r, bubbleData.color.g, bubbleData.color.b, 0.5f); ;

            transform.localScale = scale;
            transform.position = startPosition;

            gameObject.SetActive(true);
            StartMovement(movementDuration, targetHeight);
        }

        public void Terminate()
        {
            _movementTween?.Kill();
            Terminated?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _idText = null;
            _spriteRenderer = null;

            _movementTween?.Kill();
            _movementTween = null;
        }

        private void StartMovement(float movementDuration, float targetHeight)
        {
            _movementTween?.Kill();
            _movementTween = transform.DOMoveY(targetHeight, movementDuration).SetEase(Ease.Linear);

            _movementTween.OnComplete(() => Terminate());
        }

        #endregion Functions
    }
}