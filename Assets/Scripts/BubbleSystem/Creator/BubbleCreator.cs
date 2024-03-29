using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Factory;
using Assets.Scripts.BubbleSystem.Creator.Data;

namespace Assets.Scripts.BubbleSystem.Creator
{
    public class BubbleCreator
    {
        #region Variables

        public Action<Bubble> BubbleCreated;
        
        #endregion Variables
        
        #region Variables

        private int _verticalOffsetIndex;

        private Transform _bubbleParent;

        private Vector3 _spawnPosition;

        private BubblePool _bubblePool;
        private BubbleFactory _bubbleFactory;
        private BubbleDataSO _bubbleDataSO;

        private List<Bubble> _activeBubbleList;
        private List<Bubble> _ceilingBubbleList;

        private BubbleCreatorSettings _settings;

        private WaitForSeconds _waitForSeconds_01;

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

        public List<Bubble> ActiveBubbleList { get => _activeBubbleList; set => _activeBubbleList = value; }
        public List<Bubble> CeilingBubbleList { get => _ceilingBubbleList; set => _ceilingBubbleList = value; }

        #endregion Properties

        #region Functions

        public BubbleCreator(BubbleDataSO bubbleDataSO, BubbleCreatorSettings settings, Transform bubbleParent)
        {
            VerticalOffsetIndex = 0;

            _settings = settings;
            _bubbleParent = bubbleParent;
            _bubbleDataSO = bubbleDataSO;

            _waitForSeconds_01 = new WaitForSeconds(0.1f);

            _bubbleFactory = new BubbleFactory(_settings.bubblePrefab);
            _bubblePool = new BubblePool(_bubbleFactory);

            _activeBubbleList = new List<Bubble>();
            _ceilingBubbleList = new List<Bubble>();
        }

        ~BubbleCreator()
        {
            _bubblePool = null;
            _bubbleFactory = null;
        }

        public void AddBubble(Bubble bubble)
        {
            if (_activeBubbleList.Contains(bubble)) return;

            _activeBubbleList.Add(bubble);
        }

        private void RemoveBubble(Bubble bubble)
        {
            _activeBubbleList.Remove(bubble);
        }

        private void AddBubbleToCeiling(Bubble bubble)
        {
            bubble.IsCeiling = true;
            _ceilingBubbleList.Add(bubble);
        }

        private void ResetCeilingBubbles()
        {
            Bubble loopBubble = null;

            for (int i = 0; i < _ceilingBubbleList.Count; i++)
            {
                loopBubble = _ceilingBubbleList[i];
                loopBubble.IsCeiling = false;
            }

            _ceilingBubbleList.Clear();
        }

        public Bubble GetBubble()
        {
            Bubble bubble = _bubblePool.GetProduct();

            bubble.transform.position = _settings.initialSpawnPosition;
            bubble.transform.SetParent(_bubbleParent, true);
            BubbleCreated?.Invoke(bubble);

            return bubble;
        }

        public IEnumerator CreateInitialPile()
        {
            _spawnPosition = _settings.initialSpawnPosition;
            _spawnPosition.y += (_settings.verticalOffset * (_settings.initialLineCount - 1));

            for (int i = 0; i < _settings.initialLineCount; i++)
            {
                CreateLinePile(true);
                yield return _waitForSeconds_01;
                _spawnPosition.y -= _settings.verticalOffset;
            }

            _spawnPosition = _settings.initialSpawnPosition;
        }

        public void CreateLinePile(bool isInitial = false)
        {
            ResetCeilingBubbles();

            Bubble instantiatedBubble = null;

            _spawnPosition.x -= 0.5f * (VerticalOffsetIndex % 2);
            VerticalOffsetIndex++;

            for (int j = 0; j < _settings.lineSize; j++)
            {
                instantiatedBubble = GetBubble();
                instantiatedBubble.transform.position = _spawnPosition;

                instantiatedBubble.MoveTo(_spawnPosition);
                instantiatedBubble.ScaleOut();

                instantiatedBubble.Initialize();
                instantiatedBubble.UpdateBubble(_bubbleDataSO.GetRandomBubbleDataByRandomMaxValue());
                instantiatedBubble.SendToPool += RemoveBubble;
                AddBubble(instantiatedBubble);

                AddBubbleToCeiling(instantiatedBubble);

                _spawnPosition.x += _settings.horizontalOffset;
            }

            _spawnPosition.x = _settings.initialSpawnPosition.x;

            if (!isInitial)
                _spawnPosition = _settings.initialSpawnPosition;
        }

        #endregion Functions
    }
}