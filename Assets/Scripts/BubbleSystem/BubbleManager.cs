using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Factory;
using Sirenix.OdinInspector;

namespace Assets.Scripts.BubbleSystem
{
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

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
            _bubblePool = new BubblePool();
            _bubbleFactory = new BubbleFactory(_bubblePrefab);
            
            _activeBubbleList = new List<Bubble>();

            CreateInitialPile();
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

        private Bubble GetBubble()
        {
            Bubble bubble = _bubblePool.GetProduct();

            if (bubble == null)
                bubble = _bubbleFactory.Manufacture();

            return bubble;
        }

        private void CreateInitialPile()
        {
            Vector3 spawnPosition = _initialSpawnPosition;

            for (int i = 0; i < _initialLineCount; i++)
            {
                spawnPosition.x = _initialSpawnPosition.x - 0.5f * (i % 2);

                CreateLinePile(spawnPosition);

                spawnPosition.y += _verticalOffset;
            }
        }

        private void CreateLinePile(Vector3 spawnPosition)
        {
            Bubble instantiatedBubble = null;

            for (int j = 0; j < _lineSize; j++)
            {
                instantiatedBubble = GetBubble();
                instantiatedBubble.transform.SetParent(transform, true);
                instantiatedBubble.transform.position = spawnPosition;

                instantiatedBubble.UpdateBubble(_bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive));
                instantiatedBubble.SendToPool += RemoveBubble;
                AddBubble(instantiatedBubble);

                spawnPosition.x += _horizontalOffset;
            }
        }

        [Button]
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

        #endregion Functions
    }
}