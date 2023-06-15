using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.BubbleSystem.Data;

namespace Assets.Scripts.BubbleSystem.MenuSceneBubbles
{
    public class SlideBubbleManager : MonoBehaviour, IDisposable
    {
        #region Events

        public delegate SlideBubble BubbleRequest();
        public BubbleRequest BubbleRequested;

        #endregion Events

        #region Variables

        private WaitForSeconds _waitForSeconds;

        private BubbleData _cachedBubbleData;
        private SlideBubble _cachedSlideBubble;

        [SerializeField] private float _coroutineDelay;

        [SerializeField] private float _endHeight;
        [SerializeField] private float _startHeight;
        [SerializeField] private float _movementDuration;

        [SerializeField] private Vector2 _scaleInterval;
        [SerializeField] private Vector2 _startPositionXInterval;

        [SerializeField] private BubbleDataSO _bubbleDataSO;

        #endregion Variables
        
        #region Properties

        private Vector3 StartPosition
        {
            get
            {
                return new Vector3(
                    UnityEngine.Random.Range(_startPositionXInterval.x, _startPositionXInterval.y),
                    _startHeight, 
                    -1f);
            }
        }

        private float Scale
        {
            get => UnityEngine.Random.Range(_scaleInterval.x, _scaleInterval.y);
        }

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
            _waitForSeconds = new WaitForSeconds(_coroutineDelay);

            StartSlideBubbles();
        }

        public void Dispose()
        {
            StopSlideBubbles();

            _waitForSeconds = null;
        }

        private float GetMovementDuration(float scale)
        {
            return _movementDuration / scale;
        }

        private void StartSlideBubbles()
        {
            StartCoroutine(SlideBubbleSpawnCoroutine());
        }

        private void StopSlideBubbles()
        {
            StopCoroutine(SlideBubbleSpawnCoroutine());
        }

        private IEnumerator SlideBubbleSpawnCoroutine()
        {
            _cachedSlideBubble = BubbleRequested();

            if (_cachedSlideBubble != null)
            {
                float scale = Scale;
                float movementDuration = GetMovementDuration(scale);

                _cachedBubbleData = _bubbleDataSO.GetRandomBubbleData();
                _cachedSlideBubble.Initialize(StartPosition, scale * Vector3.one, movementDuration, _endHeight, _cachedBubbleData);

                yield return _waitForSeconds;

                StartSlideBubbles();
            }
            else
            {
                yield return _waitForSeconds;
                StartSlideBubbles();
            }
        }

        #endregion Functions
    }
}