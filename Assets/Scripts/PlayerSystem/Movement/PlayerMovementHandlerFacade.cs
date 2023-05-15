using System;
using UnityEngine;

namespace Assets.Scripts.PlayerSystem.Movement
{
    public class PlayerMovementHandlerFacade : MonoBehaviour, IDisposable
    {
        #region Variables

        private bool _isPlayerMovable;

        private PlayerMovementHandler _playerMovementHandler;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Awake - Update - OnDisable

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            MovePlayerVertically();
        }

        private void OnDisable()
        {
            Terminate();
            Dispose();
        }

        #endregion Awake - Update - OnDisable

        #region Functions

        public void PlayerMovementHandlerFacadeConstructor(PlayerMovementHandler playerMovementHandler)
        {
            _playerMovementHandler = playerMovementHandler;
        }

        private void Initialize()
        {
            SubscribeEvents(true);
        }

        private void Terminate()
        {
            SubscribeEvents(false);
        }

        public void Dispose()
        {
            _playerMovementHandler = null;
        }

        private void SubscribeEvents(bool subscribe)
        {
            if (subscribe)
            {
                
            }
            else if (!subscribe)
            {
                
            }
        }

        private void OnPlayButtonClickedSignal()
        {
            enabled = true;
            _isPlayerMovable = true;
        }

        private void MovePlayerVertically()
        {
            if (!_isPlayerMovable) return;

            _playerMovementHandler.MovePlayerVertically();
        }

        private void MovePlayerHorizontally()
        {
            if (!_isPlayerMovable) return;

            _playerMovementHandler.MovePlayerHorizontally();
        }

        #endregion Functions
    }
}