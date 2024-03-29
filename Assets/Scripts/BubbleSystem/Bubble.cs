using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Assets.Scripts.HapticSystem;
using Assets.Scripts.ProductSystem;
using Assets.Scripts.ThrowSystem.Data;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.BubbleSystem
{
    public class Bubble : MonoBehaviour, IProduct<Bubble>
    {
        #region Events

        public Action<Bubble> ThrowEvent;
        public Action<Bubble> DisposeEvent;
        public Action<Bubble> ExplodeEvent;

        #endregion Events

        #region Variables

        private const int FinalId = 2048;

        private bool _isCeiling;
        private bool _isDisposed;
        private bool _isExploded;
        private bool _isThrowBubble;

        private Tween _scaleTween;
        private Tween _movementTween;
        private Sequence _throwSequence;

        private Vector3 _currentPosition;

        private Bubble _cachedTempBubble;
        private BubbleData _bubbleData;

        private Collider[] _neighbourColliders;
        private Collider[] _emptyNeighbourColliders;
        private List<Bubble> _neighbourBubbleList;

        private readonly List<Vector3> _neighbourOffsetList = new List<Vector3>
        {
            new Vector3(0.5f, 0.88f, 0f),
            new Vector3(-0.5f, 0.88f, 0f),

            new Vector3(0.5f, -0.88f, 0f),
            new Vector3(-0.5f, -0.88f, 0f),

            new Vector3(1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
        };

        [SerializeField] private ThrowSettings _throwSettings;

        [SerializeField] private float _fallTargetHeight;
        [SerializeField] private float _fallDurationMultiplier;

        [SerializeField] private float _shakeAmount;
        [SerializeField] private float _shakeDuration;

        [SerializeField] private LayerMask _bubbleWallMask;

        [SerializeField] private Collider _collider;
        [SerializeField] private TextMeshPro _idText;
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion Variables

        #region Properties

        public bool IsCeiling { get => _isCeiling; set => _isCeiling = value; }
        public bool IsDisposed { get => _isDisposed; set => _isDisposed = value; }
        public bool IsExploded { get => _isExploded; set => _isExploded = value; }
        public bool IsThrowBubble { get => _isThrowBubble; set => _isThrowBubble = value; }
        public BubbleData BubbleData { get => _bubbleData; set => _bubbleData = value; }
        public Action<Bubble> SendToPool { get => DisposeEvent; set => DisposeEvent = value; }
        public TrailRenderer TrailRenderer { get => _trailRenderer; }

        #endregion Properties

        #region Functions

        public void Initialize()
        {
            _isDisposed = false;
            _isExploded = false;
            _trailRenderer.enabled = false;

            _neighbourColliders = new Collider[9];
            _emptyNeighbourColliders = new Collider[1];
            _neighbourBubbleList = new List<Bubble>();

            _collider.enabled = true;
            gameObject.SetActive(true);
        }

        public void Dispose()
        {
            _isCeiling = false;
            _isDisposed = true;

            transform.localScale = Vector3.zero;

            _scaleTween?.Kill();
            _throwSequence?.Kill();
            _movementTween?.Kill();

            _bubbleData = null;

            _neighbourColliders = null;
            _emptyNeighbourColliders = null;

            _idText.alpha = 1f;

            _collider.enabled = false;
            DisposeEvent?.Invoke(this);
            gameObject.SetActive(false);
        }

        private void UpdateBubble()
        {
            SetText("" + _bubbleData.id);
            SetColor(_bubbleData.color, true);
        }

        public void UpdateBubble(BubbleData bubbleData)
        {
            _bubbleData = bubbleData;
            UpdateBubble();

            ExplodeBubbleIfPossible();
        }

        private void ExplodeBubbleIfPossible()
        {
            if (_bubbleData.id != FinalId) return;

            ExplodeBubble();
        }

        private void ExplodeBubble()
        {
            _isExploded = true;
            ExplodeEvent?.Invoke(this);
        }

        private void SetText(string text)
        {
            _idText.text = text;
        }

        public void SetColor(Color color, bool withTween, float duration = 0.2f)
        {
            if (!withTween)
            {
                _spriteRenderer.color = color;
            }
            else if (withTween)
            {
                _spriteRenderer.DOColor(color, duration);
            }
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Fall(Action action = null)
        {
            _collider.enabled = false;
            float duration = Mathf.Abs(transform.position.y - _fallTargetHeight) * _fallDurationMultiplier;
            
            transform.DOMoveY(_fallTargetHeight, duration).SetEase(Ease.InCubic).OnComplete(() =>
            {
                action?.Invoke();
                Dispose();
            });
        }

        public bool IsFallable()
        {
            if (IsCeiling) return false;

            _neighbourBubbleList = GetNeighbourBubbles();
            List<Bubble> checkedBubbleList = new List<Bubble>();
            checkedBubbleList.Add(this);

            List<Bubble> tempList = new List<Bubble>();

            for (int i = 0; i < _neighbourBubbleList.Count; i++)
            {
                _cachedTempBubble = _neighbourBubbleList[i];

                if (_cachedTempBubble.IsCeiling)
                {
                    _neighbourBubbleList.Clear();
                    return false;
                }

                if (!checkedBubbleList.Contains(_cachedTempBubble))
                {
                    tempList = _cachedTempBubble.GetNeighbourBubbles();
                    for (int j = 0; j < tempList.Count; j++)
                    {
                        if (!_neighbourBubbleList.Contains(tempList[j]))
                        {
                            _neighbourBubbleList.Add(tempList[j]);
                        }
                    }
                    checkedBubbleList.Add(_cachedTempBubble);
                }
            }

            _cachedTempBubble = null;
            _neighbourBubbleList.Clear();

            return true;
        }

        public void MoveTo(Vector3 targetPosition, float duration = 0.25f)
        {
            _currentPosition = targetPosition;

            _movementTween?.Kill();
            _movementTween = transform.DOMove(_currentPosition, duration);
        }

        public void MoveToDispose(Vector3 targetPosition, float duration = 0.2f)
        {
            _collider.enabled = false;
            _currentPosition = targetPosition;
            transform.position += Vector3.forward * 0.05f;

            _throwSequence?.Kill();
            _throwSequence = DOTween.Sequence();
            _throwSequence
                 .Append(transform.DOMove(_currentPosition, duration))
                 .Join(_idText.DOFade(0f, duration / 2f));

            _throwSequence.OnComplete(() =>
            {
                Dispose();
            });
        }

        public void MoveDown(float amount, float duration = 0.2f)
        {
            _currentPosition.y += amount;

            _movementTween?.Kill();
            _movementTween = transform.DOMoveY(_currentPosition.y, duration);
        }

        public void Throw(Vector3[] targetPositions)
        {
            _currentPosition = targetPositions[^1];
            _trailRenderer.enabled = true;

            float distance;
            float throwDuration = 0.2f;

            _throwSequence?.Kill();
            _throwSequence = DOTween.Sequence();

            for (int i = 0; i < targetPositions.Length; i++)
            {
                if (i == 0)
                    distance = Vector3.Distance(transform.position, targetPositions[i]);
                else
                    distance = Vector3.Distance(targetPositions[i - 1], targetPositions[i]);

                throwDuration = _throwSettings.GetThrowDuration(distance);

                _throwSequence.Append(transform.DOMove(targetPositions[i], throwDuration).SetEase(Ease.Linear));
            }

            _throwSequence.OnComplete(() =>
            {
                ShakeNeighbourBubbles();
                _trailRenderer.enabled = false;
                HapticExtensions.PlayLightHaptic();

                ThrowEvent?.Invoke(this);
            });
        }

        public void ScaleOut(float amount = 1f, float duration = 0.25f, float delay = 0f)
        {
            transform.localScale = Vector3.zero;

            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(amount, duration).SetDelay(delay);
        }

        public void ShakeBubble(Vector3 direction)
        {
            _movementTween.Kill();
            _movementTween = transform.DOPunchPosition(direction * _shakeAmount, _shakeDuration);
        }

        private void ShakeNeighbourBubbles()
        {
            _neighbourBubbleList = GetNeighbourBubbles();
            if (_neighbourBubbleList.Count <= 0) return;

            Vector3 direction;

            for (int i = 0; i < _neighbourBubbleList.Count; i++)
            {
                _cachedTempBubble = _neighbourBubbleList[i];

                direction = _cachedTempBubble.transform.position - transform.position;
                direction = direction.normalized;

                _cachedTempBubble.ShakeBubble(direction);
            }

            _cachedTempBubble = null;
            _neighbourBubbleList.Clear();
        }

        public List<Bubble> GetNeighbourBubbles()
        {
            Collider loopCollider = null;
            List<Bubble> bubbleList = new List<Bubble>();

            _neighbourColliders = new Collider[9];
            int count = Physics.OverlapSphereNonAlloc(transform.position, 1.2f, _neighbourColliders);
            
            for (int i = 0; i < count; i++)
            {
                loopCollider = _neighbourColliders[i];
                loopCollider.TryGetComponent(out _cachedTempBubble);
                if (_cachedTempBubble != null && !_cachedTempBubble.IsThrowBubble && _cachedTempBubble != this)
                {
                    bubbleList.Add(_cachedTempBubble);
                }
            }

            _cachedTempBubble = null;

            return bubbleList;
        }

        public List<Bubble> GetNeighbourBubblesWithSameId()
        {
            List<Bubble> neighbourList = GetNeighbourBubbles();

            for (int i = neighbourList.Count - 1; i >= 0; i--)
            {
                _cachedTempBubble = neighbourList[i];

                if (_cachedTempBubble._bubbleData.id != _bubbleData.id)
                {
                    neighbourList.RemoveAt(i);
                }
            }

            _cachedTempBubble = null;

            return neighbourList;
        }

        public List<Bubble> GetNeighbourBubblesWithDifferentId()
        {
            List<Bubble> neighbourList = GetNeighbourBubbles();

            for (int i = neighbourList.Count - 1; i >= 0; i--)
            {
                _cachedTempBubble = neighbourList[i];

                if (_cachedTempBubble.BubbleData != null && _bubbleData != null && _cachedTempBubble._bubbleData.id == _bubbleData.id)
                {
                    neighbourList.RemoveAt(i);
                }
            }

            _cachedTempBubble = null;

            return neighbourList;
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

                _emptyNeighbourColliders = new Collider[1];
                overlapCount = Physics.OverlapBoxNonAlloc(position, Vector3.one * 0.25f, _emptyNeighbourColliders, Quaternion.identity, _bubbleWallMask);
                
                if (overlapCount == 0)
                {
                    emptyPositions.Add(position);
                }
            }

            return emptyPositions;
        }

        public bool IsMergable()
        {
            _neighbourBubbleList = GetNeighbourBubblesWithSameId();
            
            for (int i = 0; i < _neighbourBubbleList.Count; i++)
            {
                _cachedTempBubble = _neighbourBubbleList[i];

                if (_cachedTempBubble.BubbleData.id == _bubbleData.id)
                {
                    return true;
                }
            }

            _cachedTempBubble = null;
            _neighbourBubbleList.Clear();

            return false;
        }

        public List<Bubble> GetMergeBubbles()
        {
            List<Bubble> finalBubbleList = new List<Bubble>();

            finalBubbleList.Add(this);

            _neighbourBubbleList = GetNeighbourBubblesWithSameId();

            for (int i = 0; i < _neighbourBubbleList.Count; i++)
            {
                _cachedTempBubble = _neighbourBubbleList[i];

                if (!_cachedTempBubble.IsThrowBubble && !finalBubbleList.Contains(_cachedTempBubble))
                {
                    finalBubbleList.Add(_cachedTempBubble);
                    AddRangeWithoutDuplicate(_neighbourBubbleList, _cachedTempBubble.GetNeighbourBubblesWithSameId());
                }
            }

            _cachedTempBubble = null;
            _neighbourBubbleList.Clear();

            return finalBubbleList;
        }

        private void AddRangeWithoutDuplicate(List<Bubble> mainList, List<Bubble> toBeAddedList)
        {
            for (int i = 0; i < toBeAddedList.Count; i++)
            {
                _cachedTempBubble = toBeAddedList[i];

                if (!mainList.Contains(_cachedTempBubble))
                {
                    mainList.Add(_cachedTempBubble);
                }
            }

            _cachedTempBubble = null;
        }

        #endregion Functions
    }
}