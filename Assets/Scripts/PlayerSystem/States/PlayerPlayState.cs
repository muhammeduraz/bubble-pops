using UnityEngine;

namespace Assets.Scripts.PlayerSystem.States
{
    [CreateAssetMenu(fileName = "PlayerPlayState", menuName = "Scriptable Objects/States/PlayerStates/PlayState")]
    public class PlayerPlayState : BasePlayerState
    {
        #region Functions

        public override void StateInitialize()
        {
            base.StateInitialize();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion Functions
    }
}