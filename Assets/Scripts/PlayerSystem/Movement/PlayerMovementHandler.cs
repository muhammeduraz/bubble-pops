using System;
using UnityEngine;
using Assets.Scripts.PlayerSystem.Data;

namespace Assets.Scripts.PlayerSystem.Movement
{
    public class PlayerMovementHandler : IDisposable
    {
        #region Variables

        private Vector3 _targetPosition;

        private Rigidbody _rigidbody;
        private PlayerMovementData _playerMovementData;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public PlayerMovementHandler(Rigidbody rigidbody, PlayerMovementData playerMovementData)
        {
            _rigidbody = rigidbody;
            _playerMovementData = playerMovementData;
        }

        public void Initialize()
        {
            SubscribeSignals(true);
        }

        public void Dispose()
        {
            SubscribeSignals(false);

            _rigidbody = null;
            _playerMovementData = null;
        }

        private void SubscribeSignals(bool subscribe)
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
            
        }

        public void MovePlayer()
        {

        }

        public void MovePlayerVertically()
        {
            
        }

        public void MovePlayerHorizontally()
        {
            
        }

        #endregion Functions
    }
}