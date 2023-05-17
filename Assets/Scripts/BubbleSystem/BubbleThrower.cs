using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.InputSystem;

namespace Assets.Scripts.BubbleSystem
{
    public class BubbleThrower : MonoBehaviour, IDisposable
    {
        #region Variables

        private Camera _camera;

        private Tween _throwTween;
        private Tween _currentBubbleTween;
        private Sequence _nextBubbleSequence;

        private InputHandler _inputHandler;
        private BubbleManager _bubbleManager;

        private Bubble _nextBubble;
        private Bubble _currentBubble;

        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private Transform _lineStartTransform;
        [SerializeField] private Transform _nextBubbleTransform;
        [SerializeField] private Transform _currentBubbleTransform;

        #endregion Variables

        #region Properties



        #endregion Properties

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

            _inputHandler = FindObjectOfType<InputHandler>();
            _bubbleManager = FindObjectOfType<BubbleManager>();

            InitializeLineRenderer();
            InitializeCurrentAndNextBubbles();

            SubscribeEvents(true);
        }

        public void Dispose()
        {
            SubscribeEvents(false);
        }

        private void SubscribeEvents(bool subscribe)
        {
            if (subscribe)
            {
                _inputHandler.OnFingerDown += OnFingerDown;    
                _inputHandler.OnFinger += OnFinger;    
                _inputHandler.OnFingerUp += OnFingerUp;    
            }
            else if (!subscribe)
            {
                _inputHandler.OnFingerDown -= OnFingerDown;
                _inputHandler.OnFinger -= OnFinger;
                _inputHandler.OnFingerUp -= OnFingerUp;
            }
        }

        private Vector3 GetScreenPosition(Vector3 mousePosition)
        {
            Vector3 targetPosition = _camera.ScreenToWorldPoint(mousePosition);
            targetPosition.z = 5f;

            return targetPosition;
        }

        private void ActivateNextBubble()
        {
            _currentBubble = _nextBubble;
            _nextBubble.transform.position -= 1f * Vector3.forward;

            _nextBubbleSequence?.Kill();
            _nextBubbleSequence = DOTween.Sequence();
            _nextBubbleSequence
                .AppendInterval(0.2f)
                .Append(_currentBubble.transform.DOScale(1f, 0.2f))
                .Append(_currentBubble.transform.DOMove(_currentBubbleTransform.position, 0.2f));
        }

        private void GetNewNextBubble()
        {
            _nextBubble = _bubbleManager.GetBubble();
            _nextBubble.Initialize();
            _nextBubble.UpdateBubble(_bubbleManager.GetRandomBubbleData);
            _nextBubble.transform.position = _nextBubbleTransform.position;
            _nextBubble.ScaleOut(0.8f, delay: 0.25f);
        }

        private void ThrowBubble(Vector3 mousePosition)
        {
            Vector3 targetPosition = GetScreenPosition(mousePosition);

            _throwTween?.Kill();
            _throwTween = _currentBubble.transform.DOMove(targetPosition, 0.2f).SetEase(Ease.Linear);
        }

        private void InitializeLineRenderer()
        {
            _lineRenderer.enabled = false;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _lineStartTransform.position);
        }

        private void UpdateLineRenderer(Vector3 mousePosition)
        {
            Vector3 targetPosition = GetScreenPosition(mousePosition);
            _lineRenderer.SetPosition(1, targetPosition);
        }

        private void InitializeCurrentAndNextBubbles()
        {
            _nextBubble = _bubbleManager.GetBubble();
            _currentBubble = _bubbleManager.GetBubble();

            _nextBubble.Initialize();
            _currentBubble.Initialize();

            _nextBubble.UpdateBubble(_bubbleManager.GetRandomBubbleData);
            _currentBubble.UpdateBubble(_bubbleManager.GetRandomBubbleData);

            _nextBubble.transform.localScale = Vector3.one * 0.8f;
            _nextBubble.transform.position = _nextBubbleTransform.position;

            _currentBubble.transform.position = _currentBubbleTransform.position;
        }

        private void OnFingerDown(Vector3 mousePosition)
        {
            UpdateLineRenderer(mousePosition);
            _lineRenderer.enabled = true;
        }

        private void OnFinger(Vector3 mousePosition)
        {
            UpdateLineRenderer(mousePosition);
        }

        private void OnFingerUp(Vector3 mousePosition)
        {
            _lineRenderer.enabled = false;

            ThrowBubble(mousePosition);

            ActivateNextBubble();
            GetNewNextBubble();
        }

        #endregion Functions
    }
}