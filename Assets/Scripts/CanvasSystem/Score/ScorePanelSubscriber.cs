using Assets.Scripts.SubscribeSystem;
using Assets.Scripts.EnvironmentSystem;

namespace Assets.Scripts.CanvasSystem.Score
{
    public class ScorePanelSubscriber : BaseSubscriber 
    {
        #region Variables

        private ScorePanel _scorePanel;

        private FailTrigger _failTrigger;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _scorePanel = FindObjectOfType<ScorePanel>();

            _failTrigger = FindObjectOfType<FailTrigger>();
        }

        protected override void Dispose()
        {
            _scorePanel = null;
            
            _failTrigger = null;

        }

        protected override void SubscribeEvents()
        {
            if (_scorePanel == null) return;

            _failTrigger.Failed += _scorePanel.Disappear;
        }

        protected override void UnSubscribeEvents()
        {
            if (_scorePanel == null) return;

            _failTrigger.Failed -= _scorePanel.Disappear;
        }

        #endregion Functions
    }
}