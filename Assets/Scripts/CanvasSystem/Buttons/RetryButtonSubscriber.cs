using Assets.Scripts.SceneSystem;
using Assets.Scripts.SubscribeSystem;

namespace Assets.Scripts.CanvasSystem.Buttons
{
    public class RetryButtonSubscriber : BaseSubscriber
    {
        #region Variables

        private RetryButton _retryButton;

        private SceneService _sceneService;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _retryButton = GetComponent<RetryButton>();

            _sceneService = FindObjectOfType<SceneService>();
        }

        protected override void Dispose()
        {
            _retryButton = null;

            _sceneService = null;
        }

        protected override void SubscribeEvents()
        {
            if (_retryButton == null || _sceneService == null) return;

            _retryButton.ReloadSceneRequested += _sceneService.ReloadCurrentLevel;
        }

        protected override void UnSubscribeEvents()
        {
            if (_retryButton == null || _sceneService == null) return;

            _retryButton.ReloadSceneRequested -= _sceneService.ReloadCurrentLevel;
        }

        #endregion Functions
    }
}