using System;
using UnityEngine;

namespace Assets.Scripts.EnvironmentSystem
{
    public class FailTrigger : MonoBehaviour
    {
        #region Events

        public Action Failed;

        #endregion Events
        
        #region Variables

        private Ray _ray;
        private RaycastHit[] _hits;

        [SerializeField] private LayerMask _raycastLayer;

        #endregion Variables

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            _ray = new Ray(transform.position, Vector3.right);
        }

        private void OnFailed()
        {
            Failed?.Invoke();
        }

        public void CheckIfFail()
        {
            int hitCount = GetHitCount();

            if (hitCount > 0)
                OnFailed();   
        }

        private int GetHitCount()
        {
            _hits = new RaycastHit[1];
            int hitCount = Physics.RaycastNonAlloc(_ray, _hits, float.PositiveInfinity, _raycastLayer);
            return hitCount;
        }

        #endregion Functions
    }
}