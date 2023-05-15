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

<<<<<<< Updated upstream
        #region Awake - Update - OnDisable
=======
        #region Unity Functions
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
        #endregion Awake - Update - OnDisable
=======
        #endregion Unity Functions
>>>>>>> Stashed changes

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