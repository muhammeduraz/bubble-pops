using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.ProductSystem;
using Assets.Scripts.BubbleSystem.Data;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

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

        private Vector3 _currentPosition;

        private BubbleData _bubbleData;

        private List<Vector3> _neighbourOffsetList = new List<Vector3>
        {
            new Vector3(0.5f, 0.88f, 0f),
            new Vector3(-0.5f, 0.88f, 0f),

            new Vector3(0.5f, -0.88f, 0f),
            new Vector3(-0.5f, -0.88f, 0f),

            new Vector3(1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
        };

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
        }

        public void Dispose()
        {
            transform.localScale = Vector3.zero;

            gameObject.SetActive(false);
        }

        private void UpdateBubble()
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
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
        
        public void MoveDown(float amount, float duration = 0.25f)
        {
            _currentPosition.y += amount;

            _movementTween?.Kill();
            _movementTween = transform.DOMoveY(_currentPosition.y, duration);
        }

        public void MoveTo(Vector3 targetPosition, float duration = 0.25f)
        {
            _currentPosition = targetPosition;

            _movementTween?.Kill();
            _movementTween = transform.DOMove(_currentPosition, duration);
        }

        public void ScaleOut(float amount = 1f, float duration = 0.25f, float delay = 0f)
        {
            transform.localScale = Vector3.zero;

            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(amount, duration).SetDelay(delay);
        }

        public List<Vector3> GetEmptyPositions()
        {
            int overlapCount = 0;

            Vector3 offset;
            Vector3 position;
            List<Vector3> emptyPositions = new List<Vector3>();

            for (int i = 0; i < _neighbourOffsetList.Count; i++)
            {
                offset = _neighbourOffsetList[i];
                position = transform.position + offset;

                overlapCount = Physics.OverlapBoxNonAlloc(position, Vector3.one * 0.25f, null);
                if (overlapCount == 0)
                {
                    emptyPositions.Add(position);
                }
            }

            return emptyPositions;
        }

        #endregion Functions
    }
}