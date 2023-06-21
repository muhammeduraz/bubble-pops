using Assets.Scripts.SubscribeSystem;

namespace Assets.Scripts.EnvironmentSystem.Theme
{
    public class BackgroundThemeHandlerSubscriber : BaseSubscriber
    {
        #region Variables

        private BackgroundThemeHandler _backgroundThemeHandler;

        private ThemeButton _themeButton;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _backgroundThemeHandler = FindObjectOfType<BackgroundThemeHandler>();
            
            _themeButton = FindObjectOfType<ThemeButton>();
        }

        protected override void Dispose()
        {
            _backgroundThemeHandler = null;
            
            _themeButton = null;
        }

        protected override void SubscribeEvents()
        {
            if (_backgroundThemeHandler == null) return;

            _themeButton.ThemeChanged += _backgroundThemeHandler.SetBackgroundSpriteColor;
        }

        protected override void UnSubscribeEvents()
        {
            if (_backgroundThemeHandler == null) return;

            _themeButton.ThemeChanged -= _backgroundThemeHandler.SetBackgroundSpriteColor;
        }

        #endregion Functions
    }
}