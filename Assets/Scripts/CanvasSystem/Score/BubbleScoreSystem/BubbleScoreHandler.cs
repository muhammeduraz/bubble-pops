using System;
using UnityEngine;

namespace Assets.Scripts.CanvasSystem.BubbleScoreSystem
{
    public class BubbleScoreHandler : MonoBehaviour, IDisposable
    {
        #region Variables

        private Camera _camera;

        private BubbleScorePool _bubbleScorePool;
        private BubbleScoreFactory _bubbleScoreFactory;

        [SerializeField] private Vector3 _offset;
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
            _camera = Camera.main;

            _bubbleScoreFactory = new BubbleScoreFactory(_bubbleScorePrefab);
            _bubbleScorePool = new BubbleScorePool(_bubbleScoreFactory);
        }

        public void Dispose()
        {
            _bubbleScorePool = null;
            _bubbleScoreFactory = null;

            _bubbleScorePrefab = null;
        }

        public void ShowScore(int id, Vector3 position)
        {
            BubbleScore bubbleScore = _bubbleScorePool.GetProduct();
            bubbleScore.transform.SetParent(transform);

            position = GetScreenPosition(position);
            bubbleScore.Appear(id, position + _offset);
        }

        private Vector3 GetScreenPosition(Vector3 worldPosition)
        {
            return _camera.WorldToScreenPoint(worldPosition); 
        }

        #endregion Functions
    }
}