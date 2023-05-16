using System;
using UnityEngine;
using Assets.Scripts.BubbleSystem.Pool;
using Assets.Scripts.BubbleSystem.Factory;

namespace Assets.Scripts.BubbleSystem
{
    public class BubbleManager : MonoBehaviour, IDisposable
    {
        #region Variables

        private BubblePool _bubblePool;
        private BubbleFactory _bubbleFactory;

        [SerializeField] private Bubble _bubblePrefab;

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
        }

        public void Dispose()
        {
            _bubblePool = null;
            _bubbleFactory = null;
        }

        #endregion Functions
    }
}