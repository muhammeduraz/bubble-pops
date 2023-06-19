using Assets.Scripts.SubscribeSystem;
using Assets.Scripts.EnvironmentSystem;
using Assets.Scripts.CanvasSystem.Score.General;

namespace Assets.Scripts.CanvasSystem.Menus
{
    public class GameFailMenuSubscriber : BaseSubscriber 
    {
        #region Variables

        private GameFailMenu _gameFailMenu;

        private FailTrigger _failTrigger;
        private GeneralScoreHandler _generalScoreHandler;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _gameFailMenu = FindObjectOfType<GameFailMenu>();

            _failTrigger = FindObjectOfType<FailTrigger>();
            _generalScoreHandler = FindObjectOfType<GeneralScoreHandler>();
        }

        protected override void Dispose()
        {
            _gameFailMenu = null;

            _failTrigger = null;
            _generalScoreHandler = null;
        }

        protected override void SubscribeEvents()
        {
            if (_gameFailMenu == null) return;

            _failTrigger.Failed += _gameFailMenu.Appear;

            _gameFailMenu.RequestGeneralScore += _generalScoreHandler.GetScore;
        }

        protected override void UnSubscribeEvents()
        {
            if (_gameFailMenu == null) return;

            _failTrigger.Failed -= _gameFailMenu.Appear;

            _gameFailMenu.RequestGeneralScore -= _generalScoreHandler.GetScore;
        }

        #endregion Functions
    }
}