using System;
using UnityEngine;

namespace Assets.Scripts.CanvasSystem.BubbleScoreSystem
{
    public class BubbleScoreHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private BubbleScorePool _bubbleScorePool;
        private BubbleScoreFactory _bubbleScoreFactory;

        [SerializeField] private BubbleScore _bubbleScorePrefab;

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
            _bubbleScoreFactory = new BubbleScoreFactory(_bubbleScorePrefab);
            _bubbleScorePool = new BubbleScorePool(_bubbleScoreFactory);
        }

        public void Dispose()
        {

        }

        #endregion Functions
    }
}