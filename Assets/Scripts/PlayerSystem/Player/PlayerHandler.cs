using System;
using Assets.Scripts.PlayerSystem.Data;

namespace Assets.Scripts.PlayerSystem
{
    public class PlayerHandler : IInitializable, IDisposable
    {
        #region Variables

        private PlayerData _playerData;

        #endregion Variables

        #region Properties



        #endregion Properties

        #region Functions

        public PlayerHandler(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void Initialize()
        {

        }

        public void Dispose()
        {
            _playerData = null;
        }

        #endregion Functions
    }
}