using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.ProductSystem;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.BubbleSystem
{
    public class Bubble : MonoBehaviour, IProduct<Bubble>
    {
        #region Events

        public Action<Bubble> DisposeEvent;

        #endregion Events

        #region Variables

        private Tween _scaleTween;
        private Tween _movementTween;

        private BubbleData _bubbleData;

        [SerializeField] private TextMeshPro _idText;
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion Variables

        #region Properties

        public BubbleData BubbleData { get => _bubbleData; set => _bubbleData = value; }
        public Action<Bubble> SendToPool { get => DisposeEvent; set => DisposeEvent = value; }
        public TrailRenderer TrailRenderer { get => _trailRenderer; }

        #endregion Properties

        #region Functions

        public void Initialize()
        {
            _trailRenderer.enabled = false;

            UpdateBubble();
        }

        public void Dispose()
        {
            transform.localScale = Vector3.zero;

            gameObject.SetActive(false);
        }

        public void UpdateBubble()
        {
            SetText("" + _bubbleData.id);
            SetColor(_bubbleData.color);
        }

        public void UpdateBubble(BubbleData bubbleData)
        {
            _bubbleData = bubbleData;
            UpdateBubble();
        }

        private void SetText(string text)
        {
            _idText.text = text;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
        
        public void MoveDown(float amount, float duration = 0.25f)
        {
            _movementTween?.Kill();
            _movementTween = transform.DOMoveY(transform.position.y + amount, duration);
        }

        public void MoveTo(Vector3 targetPosition, float duration = 0.25f)
        {
            _movementTween?.Kill();
            _movementTween = transform.DOMove(targetPosition, duration);
        }

        public void ScaleOut(float duration = 0.25f)
        {
            transform.localScale = Vector3.zero;

            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(1f, duration);
        }

        #endregion Functions
    }
}