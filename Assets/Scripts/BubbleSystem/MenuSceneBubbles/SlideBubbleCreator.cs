using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.BubbleSystem.MenuSceneBubbles
{
    [DefaultExecutionOrder(-1)]
    public class SlideBubbleCreator : MonoBehaviour, IDisposable
    {
        #region Variables

        [SerializeField] private SlideBubble _slideBubblePrefab;

        private SlideBubble _cachedSlideBubble;
        private Stack<SlideBubble> _slideBubbleStack;

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
            _slideBubbleStack = new Stack<SlideBubble>();

            CreateSlideBubbles(20);
        }

        public void Dispose()
        {
            _slideBubblePrefab = null;

            _slideBubbleStack = null;
        }
        
        private void OnSlideBubbleTerminated(SlideBubble slideBubble)
        {
            _slideBubbleStack.Push(slideBubble);
            slideBubble.Terminated -= OnSlideBubbleTerminated;
        }

        private void CreateSlideBubbles(int amount)
        {
            SlideBubble loopSlideBubble = null;

            for (int i = 0; i < amount; i++)
            {
                loopSlideBubble = Instantiate(_slideBubblePrefab, transform);
                loopSlideBubble.gameObject.SetActive(false);

                _slideBubbleStack.Push(loopSlideBubble);
            }
        }

        public SlideBubble OnSlideBubbleRequested()
        {
            _slideBubbleStack.TryPop(out _cachedSlideBubble);

            if (_cachedSlideBubble != null)
            {
                _cachedSlideBubble.Terminated += OnSlideBubbleTerminated;
            }

            return _cachedSlideBubble;
        }

        #endregion Functions
    }
}