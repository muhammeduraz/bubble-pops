using Assets.Scripts.SubscribeSystem;
using Assets.Scripts.EnvironmentSystem;

namespace Assets.Scripts.InputSystem
{
    public class InputHandlerSubscriber : BaseSubscriber 
    {
        #region Variables

        private InputHandler _inputHandler;

        private FailTrigger _failTrigger;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _inputHandler = FindObjectOfType<InputHandler>();

            _failTrigger = FindObjectOfType<FailTrigger>();
        }

        protected override void Dispose()
        {
            _inputHandler = null;

            _failTrigger = null;
        }

        protected override void SubscribeEvents()
        {
            if (_inputHandler == null) return;

            _failTrigger.Failed += _inputHandler.Deactivate;
        }

        protected override void UnSubscribeEvents()
        {
            if (_inputHandler == null) return;

            _failTrigger.Failed -= _inputHandler.Deactivate;
        }

        #endregion Functions
    }
}