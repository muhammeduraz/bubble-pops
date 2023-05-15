namespace Assets.Scripts.GameSystem.States
{
    public interface IGameState
    {
        #region Functions

        public void Initialize();

        public void OnStateEnter();

        public void OnStateExit();

        #endregion Functions
    }
}