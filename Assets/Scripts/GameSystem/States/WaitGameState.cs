using UnityEngine;

namespace Assets.Scripts.GameSystem.States
{
    [CreateAssetMenu(fileName = "WaitGameState", menuName = "Scriptable Objects/States/GameStates/WaitGameState")]
    public class WaitGameState : BasePlayerState
    {
        #region Functions

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