using UnityEngine;
using Assets.Scripts.PlayerSystem;
using Assets.Scripts.PlayerSystem.Data;
using Assets.Scripts.PlayerSystem.Movement;
using Assets.Scripts.PlayerSystem.Animation;

namespace Assets.Scripts.PlayerSystemzxw
{
    public class DIPlayer : MonoBehaviour
    {
        #region Variables

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private PlayerMovementData _playerMovementData;

        [SerializeField] private PlayerHandler _playerHandler;
        [SerializeField] private PlayerMovementHandler _playerMovementHandler;
        [SerializeField] private PlayerAnimationHandler _playerAnimationHandler;

        #endregion Variables

        #region Functions

        public void ManageDependencies()
        {
            
        }

        #endregion Functions
    }
}