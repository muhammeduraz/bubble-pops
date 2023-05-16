using System;
using UnityEngine;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Data;
using Assets.Scripts.BubbleSystem.Factory;

namespace Assets.Scripts.BubbleSystem
{
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

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

            CreateInitialPile();
        }

        public void Dispose()
        {
            _bubblePool = null;
            _bubbleFactory = null;
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
            Bubble loopBubble = null;
            Vector3 spawnPosition = _initialSpawnPosition;

            for (int i = 0; i < _initialLineCount; i++)
            {
                spawnPosition.x += -0.5f * (i % 2);

                for (int j = 0; j < _lineSize; j++)
                {
                    loopBubble = GetBubble();
                    loopBubble.transform.SetParent(transform, true);
                    loopBubble.transform.position = spawnPosition;

                    loopBubble.UpdateBubble(_bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive));

                    spawnPosition.x += _horizontalOffset;
                }

                spawnPosition.y += _verticalOffset;
                spawnPosition.x = _initialSpawnPosition.x;
            }
        }

        private void CreateLinePile()
        {
            Bubble loopBubble = null;
            Vector3 spawnPosition = _initialSpawnPosition;

            for (int j = 0; j < _lineSize; j++)
            {
                loopBubble = GetBubble();
                loopBubble.transform.SetParent(transform, true);
                loopBubble.transform.position = spawnPosition;

                loopBubble.UpdateBubble(_bubbleDataSO.GetRandomBubbleData(_randomMaxExclusive));

                spawnPosition.x += _horizontalOffset;
            }
        }

        private void MoveAllBubblesDown()
        {

        }

        #endregion Functions
    }
}