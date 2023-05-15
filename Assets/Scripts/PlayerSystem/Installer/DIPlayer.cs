using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.PlayerSystem;
using Assets.Scripts.PlayerSystem.Data;
using Assets.Scripts.PlayerSystem.Movement;
using Assets.Scripts.PlayerSystem.Animation;

namespace Assets.Scripts.PlayerSystemzxw
{
    public class DIPlayer : MonoBehaviour
    {
        #region Variables

        [BoxGroup("Data")][SerializeField] private PlayerData _playerData;
        [BoxGroup("Data")][SerializeField] private PlayerMovementData _playerMovementData;

        [BoxGroup("Handlers")][SerializeField] private PlayerHandler _playerHandler;
        [BoxGroup("Handlers")][SerializeField] private PlayerMovementHandler _playerMovementHandler;
        [BoxGroup("Handlers")][SerializeField] private PlayerAnimationHandler _playerAnimationHandler;

        #endregion Variables

        #region Functions

        public void ManageDependencies()
        {
            
        }

        #endregion Functions
    }
}