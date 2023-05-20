using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.InputSystem;
using System.Collections.Generic;
using Assets.Scripts.EnvironmentSystem;

namespace Assets.Scripts.BubbleSystem
{
    public class BubbleThrower : MonoBehaviour, IDisposable
    {
        #region Variables

        private bool _isFingerDown;
        private bool _isThrowActive;

        private Camera _camera;

        private Tween _throwTween;
        private Sequence _currentBubbleSequence;

        private InputHandler _inputHandler;
        private BubbleManager _bubbleManager;

        private Bubble _nextBubble;
        private Bubble _currentBubble;

        private Vector3 _targetPosition;

        [SerializeField] private float _minDirectionY;

        [SerializeField] private ThrowGuide _throwGuide;
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private Transform _rayStartTransform;
        [SerializeField] private Transform _lineStartTransform;
        [SerializeField] private Transform _nextBubbleTransform;
        [SerializeField] private Transform _currentBubbleTransform;

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
            _isThrowActive = true;

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

            _throwGuide.SetColor(_currentBubble.BubbleData.color);

            _currentBubbleSequence?.Kill();
            _currentBubbleSequence = DOTween.Sequence();
            _currentBubbleSequence
                .AppendInterval(0.2f)
                .Append(_currentBubble.transform.DOScale(1f, 0.2f))
                .Append(_currentBubble.transform.DOMove(_currentBubbleTransform.position, 0.2f));

            _currentBubbleSequence.OnComplete(() => _isThrowActive = true);
        }

        private void GetNewNextBubble()
        {
            _nextBubble = _bubbleManager.GetBubble();

            _nextBubble.Initialize();
            _nextBubble.SetPosition(_nextBubbleTransform.position);
            _nextBubble.UpdateBubble(_bubbleManager.GetRandomBubbleData);
            
            _nextBubble.ScaleOut(0.8f, delay: 0.25f);
        }

        private void ThrowBubble(Vector3 targetPosition)
        {
            _isThrowActive = false;

            _currentBubble.Throw(targetPosition, 0.2f);
        }

        private void InitializeLineRenderer()
        {
            _lineRenderer.enabled = false;

            _lineRenderer.positionCount = 3;
            _lineRenderer.SetPosition(0, _lineStartTransform.position);
            _lineRenderer.SetPosition(1, _lineStartTransform.position);
            _lineRenderer.SetPosition(2, _lineStartTransform.position);
        }

        private void UpdateLineRenderer(int index, Vector3 targetPosition)
        {
            _lineRenderer.SetPosition(index, targetPosition);
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

            _throwGuide.SetColor(_currentBubble.BubbleData.color);
        }

        private Vector3 ClampDirection(Vector3 direction)
        {
            if (direction.y < _minDirectionY)
                direction.y = _minDirectionY;

            return direction;
        }

        private void FireRay(Vector3 mousePosition)
        {
            Vector3 targetPosition = GetScreenPosition(mousePosition);
            Vector3 direction = targetPosition - _rayStartTransform.position;
            direction = direction.normalized;

            direction = ClampDirection(direction);

            bool isHit = Physics.Raycast(_rayStartTransform.position, direction, out RaycastHit hit);

            if (!isHit) return;

            hit.collider.TryGetComponent(out Wall wall);
            if(wall != null)
            {
                UpdateLineRenderer(1, hit.point);

                direction.x *= -1f;
                isHit = Physics.Raycast(hit.point, direction, out hit);

                if (!isHit) return;

                hit.collider.TryGetComponent(out Bubble bubble);
                if (bubble != null)
                {
                    UpdateLineRenderer(2, hit.point);
                    UpdateThrowGuide(bubble, hit.point);
                }
                else
                {
                    hit.collider.TryGetComponent(out wall);
                    if (wall != null)
                    {
                        UpdateLineRenderer(2, _lineRenderer.GetPosition(1));
                    }
                }
            }
            else
            {
                hit.collider.TryGetComponent(out Bubble bubble);
                if (bubble != null)
                {
                    UpdateLineRenderer(1, _rayStartTransform.position);
                    UpdateLineRenderer(2, hit.point);

                    UpdateThrowGuide(bubble, hit.point);
                }
            }
        }

        private void UpdateThrowGuide(Bubble bubble, Vector3 point)
        {
            Vector3 guidePosition = GetClosestPosition(bubble, point);

            if (guidePosition != _targetPosition)
            {
                _targetPosition = guidePosition;

                _throwGuide.SetPosition(guidePosition);
                _throwGuide.ScaleOut();
            }
        }

        private Vector3 GetClosestPosition(Bubble bubble, Vector3 hitPoint)
        {
            List<Vector3> positionList = bubble.GetEmptyPositions();

            float tempDistance;
            float closestDistance = float.MaxValue;
            
            Vector3 loopPosition;
            Vector3 closestPosition = Vector3.zero;

            for (int i = 0; i < positionList.Count; i++)
            {
                loopPosition = positionList[i];
                tempDistance = Vector3.Distance(hitPoint, loopPosition);
                if (tempDistance < closestDistance)
                {
                    closestDistance = tempDistance;
                    closestPosition = loopPosition;
                }
            }

            return closestPosition; 
        }

        private void OnFingerDown(Vector3 mousePosition)
        {
            if (!_isThrowActive) return;
            _isFingerDown = true;

            _lineRenderer.enabled = true;
        }

        private void OnFinger(Vector3 mousePosition)
        {
            if (!_isThrowActive || !_isFingerDown) return;

            FireRay(mousePosition);
        }

        private void OnFingerUp(Vector3 mousePosition)
        {
            if (!_isThrowActive || !_isFingerDown) return;
            _isFingerDown = false;
            
            _lineRenderer.enabled = false;

            ThrowBubble(_targetPosition);

            ActivateNextBubble();
            GetNewNextBubble();

            _throwGuide.Reset();
        }

        #endregion Functions
    }
}