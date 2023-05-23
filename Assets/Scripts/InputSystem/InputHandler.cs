using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Assets.Scripts.InputSystem
{
    public class InputHandler : MonoBehaviour, IDisposable
    {
        #region Events

        public Action<Vector3> OnFingerDown;
        public Action<Vector3> OnFinger;
        public Action<Vector3> OnFingerUp;

        #endregion Events

        #region Variables

        private bool _isEnabled;

        private PointerEventData _eventData;

        [SerializeField] private LayerMask _uILayerMask;

        #endregion Variables

        #region Properties

        public bool IsEnabled { get => _isEnabled; set { _isEnabled = value; enabled = value; } }

        public Vector3 MousePosition { get => Input.mousePosition; }

        #endregion Properties

        #region Unity Functions

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            UpdateInput();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion Unity Functions

        #region Functions

        private void Initialize()
        {
            _eventData = new PointerEventData(EventSystem.current);
        }

        public void Dispose()
        {

        }

        private void UpdateInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsOverGUI()) return;

                OnFingerDown?.Invoke(MousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnFinger?.Invoke(MousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnFingerUp?.Invoke(MousePosition);
            }
        }

        private bool IsOverGUI()
        {
            RaycastResult result;
            List<RaycastResult> raycastResults = GetRaycastResults();

            for (int i = 0; i < raycastResults.Count; i++)
            {
                result = raycastResults[i];
                if (_uILayerMask == (_uILayerMask | (1 << result.gameObject.layer)))
                {
                    return true;
                }
            }

            return false;
        }

        private List<RaycastResult> GetRaycastResults()
        {
            _eventData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventData, raycastResults);
            return raycastResults;
        }

        #endregion Functions
    }
}