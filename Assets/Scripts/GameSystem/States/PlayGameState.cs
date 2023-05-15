using UnityEngine;

namespace Assets.Scripts.GameSystem.States
{
    [CreateAssetMenu(fileName = "PlayGameState", menuName = "Scriptable Objects/States/GameStates/PlayGameState")]
    public class PlayerPlayState : BasePlayerState
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