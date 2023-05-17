using System;
using UnityEngine;

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

        }

        public void Dispose()
        {

        }

        private void UpdateInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
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

        #endregion Functions
    }
}