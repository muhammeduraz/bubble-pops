using System;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace Assets.Scripts.CameraSystem
{
    public class CameraService : MonoBehaviour, IDisposable
    {
        #region Variables

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _gameCamera;

        [SerializeField] private float _shakeDuration;
        [SerializeField] private float _shakeStrength;
        [SerializeField] private int _shakeVibrato;

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
            _gameCamera.transform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
        }

        #endregion Functions
    }
}