using System;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using Assets.Scripts.CameraSystem.Data;

namespace Assets.Scripts.CameraSystem
{
    public class CameraService : MonoBehaviour, IDisposable
    {
        #region Variables

        private Vector3 _initialPosition;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _gameCamera;

        [SerializeField] private CameraSettings _settings;

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

        }

        public void Dispose()
        {
            _mainCamera = null;
            _gameCamera = null;
        }

        public void ShakeCamera()
        {
            _gameCamera.transform.DOShakePosition(_settings.shakeDuration, _settings.shakeStrength, _settings.shakeVibrato)
                .OnComplete(()=> SetInitialPosition());
        }

        private void SetInitialPosition()
        {
            transform.position = _initialPosition;
        }

        #endregion Functions
    }
}