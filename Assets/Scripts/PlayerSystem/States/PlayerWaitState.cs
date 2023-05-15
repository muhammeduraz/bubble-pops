using UnityEngine;

namespace Assets.Scripts.PlayerSystem.States
{
    [CreateAssetMenu(fileName = "PlayerWaitState", menuName = "Scriptable Objects/States/PlayerStates/WaitState")]
    public class PlayerWaitState : BasePlayerState
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