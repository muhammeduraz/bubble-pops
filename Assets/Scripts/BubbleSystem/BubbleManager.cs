using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Factory;

namespace Assets.Scripts.BubbleSystem
{
    [DefaultExecutionOrder(-1)]
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

        private int _verticalOffsetIndex;

        private List<Bubble> _activeBubbleList;

        private BubblePool _bubblePool;
        private BubbleFactory _bubbleFactory;

        [SerializeField] private float _verticalOffset;
        [SerializeField] private float _horizontalOffset;
        [SerializeField] private Vector3 _initialSpawnPosition;

        [SerializeField] private int _lineSize;
        [SerializeField] private int _initialLineCount;

        [SerializeField] private int _randomMaxExclusive;
        [SerializeField] private Bubble _bubblePrefab;
        [SerializeField] private BubbleDataSO _bubbleDataSO;

        #endregion Variables

        #region Properties

        private int VerticalOffsetIndex 
        { 
            get => _verticalOffsetIndex;
            set 
            {
                if (value == 2) value = 0;
                _verticalOffsetIndex = value;
            } 
        }

        public BubbleData GetRandomBubbleData { get => _bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive); }

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
            VerticalOffsetIndex = 0;

            _bubblePool = new BubblePool();
            _bubbleFactory = new BubbleFactory(_bubblePrefab);
            
            _activeBubbleList = new List<Bubble>();

            StartCoroutine(CreateInitialPile());
        }

        public void Dispose()
        {
            _bubblePool = null;
            _bubbleFactory = null;
        }

        private void AddBubble(Bubble bubble)
        {
            if (_activeBubbleList.Contains(bubble)) return;

            _activeBubbleList.Add(bubble);
        }

        private void RemoveBubble(Bubble bubble)
        {
            _activeBubbleList.Remove(bubble);
        }

        public Bubble GetBubble()
        {
            Bubble bubble = _bubblePool.GetProduct();

            if (bubble == null)
                bubble = _bubbleFactory.Manufacture();

            bubble.transform.SetParent(transform, true);
            return bubble;
        }

        private IEnumerator CreateInitialPile()
        {
            Vector3 spawnPosition = _initialSpawnPosition;

            for (int i = 0; i < _initialLineCount; i++)
            {
                StartCoroutine(CreateLinePile(spawnPosition));
                yield return new WaitForSeconds(0.1f);
                spawnPosition.y += _verticalOffset;
            }
        }

        private IEnumerator CreateLinePile(Vector3 spawnPosition)
        {
            Bubble instantiatedBubble = null;
            spawnPosition.x -= 0.5f * (VerticalOffsetIndex % 2);
            VerticalOffsetIndex++;

            for (int j = 0; j < _lineSize; j++)
            {
                instantiatedBubble = GetBubble();
                instantiatedBubble.transform.position = spawnPosition + Vector3.down * _verticalOffset;

                instantiatedBubble.MoveTo(spawnPosition);
                instantiatedBubble.ScaleOut();

                instantiatedBubble.Initialize();
                instantiatedBubble.UpdateBubble(_bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive));
                instantiatedBubble.SendToPool += RemoveBubble;
                AddBubble(instantiatedBubble);

                spawnPosition.x += _horizontalOffset;

                yield return new WaitForSeconds(0.05f);
            }
        }

        private void MoveAllBubblesDown()
        {
            Bubble loopBubble = null;

            for (int i = 0; i < _activeBubbleList.Count; i++)
            {
                loopBubble = _activeBubbleList[i];

                if (loopBubble != null)
                {
                    loopBubble.MoveDown(_verticalOffset);
                }
            }
        }

        [Button]
        private void MoveDownAndCreateLine()
        {
            MoveAllBubblesDown();
            StartCoroutine(CreateLinePile(_initialSpawnPosition));
        }

        #endregion Functions
    }
}