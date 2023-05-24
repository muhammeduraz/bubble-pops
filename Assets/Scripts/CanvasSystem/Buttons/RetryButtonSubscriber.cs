using Assets.Scripts.SceneSystem;
using Assets.Scripts.SubscriberSystem;

namespace Assets.Scripts.CanvasSystem.Buttons
{
    public class RetryButtonSubscriber : BaseSubscriber
    {
        #region Variables

        private RetryButton _retryButton;

        private SceneService _sceneService;

        #endregion Variables

        #region Functions

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _retryButton = FindObjectOfType<RetryButton>();
            _sceneService = FindObjectOfType<SceneService>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _retryButton = null;
            _sceneService = null;
        }

        protected override void Subscribe()
        {
            if (_retryButton == null || _sceneService == null) return;

            _retryButton.ReloadSceneRequested += _sceneService.ReloadCurrentLevel;
        }

        protected override void UnSubscribe()
        {
            if (_retryButton == null || _sceneService == null) return;

            _retryButton.ReloadSceneRequested -= _sceneService.ReloadCurrentLevel;
        }

        #endregion Functions
    }
}