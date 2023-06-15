using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Assets.Scripts.BubbleSystem;
using Assets.Scripts.HapticSystem;
using Assets.Scripts.EnvironmentSystem;
using Assets.Scripts.BubbleSystem.Data;
using System.Security.Cryptography;

namespace Assets.Scripts.ThrowSystem
{
    public class BubbleThrower : MonoBehaviour, IDisposable
    {
        #region Events

        public delegate Bubble ThrowBubbleRequest();
        public ThrowBubbleRequest ThrowBubbleRequested;

        public delegate BubbleData BubbleDataRequest();
        public BubbleDataRequest BubbleDataRequested;

        #endregion Events

        #region Variables

        private bool _isThrowable;
        private bool _isFingerDown;
        private bool _isMergeCompleted;
        private bool _isNextBubbleReady;

        private Camera _camera;

        private Bubble _nextBubble;
        private Bubble _targetBubble;
        private Bubble _currentBubble;

        private Sequence _currentBubbleSequence;

        private Vector3 _targetPosition;
        private Vector3[] _cachedTargetPositions;
        private List<Vector3> _cachedPositionList;

        [SerializeField] private LayerMask _wallLayerMask;
        [SerializeField] private LayerMask _bubbleLayerMask;

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
            _isMergeCompleted = true;
            _isNextBubbleReady = true;

            InitializeLineRenderer();
            InitializeCurrentAndNextBubbles();
        }

        public void Dispose()
        {
            _throwGuide = null;
            _lineRenderer = null;
            _rayStartTransform = null;
            _lineStartTransform = null;
            _nextBubbleTransform = null;
            _currentBubbleTransform = null;
        }

        public void OnMergeOperationCompleted()
        {
            _isMergeCompleted = true;
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

            _currentBubbleSequence.OnComplete(() => _isNextBubbleReady = true);
        }

        private void GetNewNextBubble()
        {
            _nextBubble = ThrowBubbleRequested();

            _nextBubble.Initialize();
            _nextBubble.SetPosition(_nextBubbleTransform.position);
            _nextBubble.UpdateBubble(BubbleDataRequested());
            
            _nextBubble.ScaleOut(0.8f, delay: 0.25f);
        }

        private void InitializeCurrentAndNextBubbles()
        {
            _nextBubble = ThrowBubbleRequested();
            _currentBubble = ThrowBubbleRequested();

            _nextBubble.Initialize();
            _currentBubble.Initialize();

            _nextBubble.UpdateBubble(BubbleDataRequested());
            _currentBubble.UpdateBubble(BubbleDataRequested());

            _nextBubble.transform.localScale = Vector3.one * 0.8f;
            _nextBubble.transform.position = _nextBubbleTransform.position;

            _currentBubble.transform.position = _currentBubbleTransform.position;

            _throwGuide.SetColor(_currentBubble.BubbleData.color);
        }

        private void CacheThrowPath()
        {
            if (_lineRenderer.GetPosition(0) == _lineRenderer.GetPosition(1) || _lineRenderer.GetPosition(1).y > _targetPosition.y)
            {
                _cachedTargetPositions = new Vector3[1];
            }
            else
            {
                _cachedTargetPositions = new Vector3[2];
                _cachedTargetPositions[0] = _lineRenderer.GetPosition(1);
            }

            _cachedTargetPositions[^1] = _targetPosition;
        }

        private void ThrowBubble()
        {
            _isMergeCompleted = false;

            CacheThrowPath();
            _currentBubble.Throw(_cachedTargetPositions);

            _targetPosition = Vector3.zero;
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
            
            bool didHit = Physics.Raycast(_rayStartTransform.position, direction, out RaycastHit hit);

            if (!didHit) return;

            hit.collider.TryGetComponent(out Wall wall);
            if(wall != null)
            {
                UpdateLineRenderer(1, hit.point);

                direction.x *= -1f;
                didHit = Physics.Raycast(hit.point, direction, out hit);

                if (!didHit) return;

                hit.collider.TryGetComponent(out Bubble bubble);
                if (bubble != null)
                {
                    UpdateLineRenderer(2, hit.point);
                    UpdateTargetPositionAndThrowGuide(bubble, hit.point);

                    _targetBubble = bubble;
                }
                else
                {
                    //hit.collider.TryGetComponent(out wall);
                    //if (wall != null)
                    //{
                    //    UpdateLineRenderer(2, _lineRenderer.GetPosition(1));
                    //}
                }
            }
            else
            {
                hit.collider.TryGetComponent(out Bubble bubble);
                if (bubble != null)
                {
                    UpdateLineRenderer(1, _rayStartTransform.position);
                    UpdateLineRenderer(2, hit.point);

                    UpdateTargetPositionAndThrowGuide(bubble, hit.point);

                    _targetBubble = bubble;
                }
            }
        }

        private void UpdateTargetPositionAndThrowGuide(Bubble bubble, Vector3 point)
        {
            Vector3 guidePosition = GetClosestPositionFromEmptyNeigbours(bubble, point);

            UpdateThrowGuide(guidePosition);
            UpdateTargetPosition(guidePosition);
        }

        private void UpdateThrowGuide(Vector3 guidePosition)
        {
            if (guidePosition != _targetPosition)
            {
                _throwGuide.SetPosition(guidePosition);
                _throwGuide.ScaleOut();
            }
        }

        private void UpdateTargetPosition(Vector3 guidePosition)
        {
            if (guidePosition == Vector3.zero)
            {
                _isThrowable = false;
                return;
            }

            _isThrowable = true;
            _targetPosition = guidePosition;
        }

        private Vector3 GetClosestPositionFromEmptyNeigbours(Bubble bubble, Vector3 hitPoint)
        {
            if (_targetBubble != bubble) 
                _cachedPositionList = bubble.GetEmptyPositions();

            float tempDistance;
            float closestDistance = float.MaxValue;
            
            Vector3 loopPosition;
            Vector3 closestPosition = Vector3.zero;

            for (int i = 0; i < _cachedPositionList.Count; i++)
            {
                loopPosition = _cachedPositionList[i];
                tempDistance = Vector3.Distance(hitPoint, loopPosition);
                if (tempDistance < closestDistance)
                {
                    closestDistance = tempDistance;
                    closestPosition = loopPosition;
                }
            }

            return closestPosition; 
        }

        public void OnFingerDown(Vector3 mousePosition)
        {
            if (!_isMergeCompleted || !_isNextBubbleReady) return;
            _isFingerDown = true;

            FireRay(mousePosition);
            _lineRenderer.enabled = true;
        }

        public void OnFinger(Vector3 mousePosition)
        {
            if (!_isMergeCompleted || !_isFingerDown) return;

            FireRay(mousePosition);
        }

        public void OnFingerUp(Vector3 mousePosition)
        {
            _lineRenderer.enabled = false;
            
            if (!_isMergeCompleted || !_isNextBubbleReady || !_isFingerDown || !_isThrowable) return;
            _isFingerDown = false;
            _isNextBubbleReady = false;

            ThrowBubble();

            ActivateNextBubble();
            GetNewNextBubble();

            _throwGuide.Reset();

            HapticExtensions.PlayLightHaptic();
        }

        #endregion Functions
    }
}