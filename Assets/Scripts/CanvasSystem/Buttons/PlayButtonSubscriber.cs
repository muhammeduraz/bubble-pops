using Assets.Scripts.SceneSystem;
using Assets.Scripts.SubscribeSystem;

namespace Assets.Scripts.CanvasSystem.Buttons
{
    public class PlayButtonSubscriber : BaseSubscriber 
    {
        #region Variables

        private PlayButton _playButton;

        private SceneService _sceneService;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _playButton = FindObjectOfType<PlayButton>();

            _sceneService = FindObjectOfType<SceneService>();
        }

        protected override void Dispose()
        {
            _playButton = null;

            _sceneService = null;
        }

        protected override void SubscribeEvents()
        {
            if (_playButton == null) return;

            _playButton.PlayButtonClicked += _sceneService.LoadGameSceneFromMenu;
        }

        protected override void UnSubscribeEvents()
        {
            if (_playButton == null) return;

            _playButton.PlayButtonClicked -= _sceneService.LoadGameSceneFromMenu;
        }

        #endregion Functions
    }
}