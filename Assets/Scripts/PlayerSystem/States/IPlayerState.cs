namespace Assets.Scripts.PlayerSystem.States
{
    public interface IPlayerState
    {
        #region Functions

        public void StateInitialize();

        public void OnStateEnter();

        public void OnStateExit();

        #endregion Functions
    }
}