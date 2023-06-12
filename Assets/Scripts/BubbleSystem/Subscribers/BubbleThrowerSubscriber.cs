using Assets.Scripts.InputSystem;
using Assets.Scripts.ThrowSystem;
using Assets.Scripts.SubscribeSystem;

namespace Assets.Scripts.BubbleSystem.Subscriber
{
    public class BubbleThrowerSubscriber : BaseSubscriber
    {
        #region Variables

        private BubbleThrower _bubbleThrower;

        private InputHandler _inputHandler;
        private BubbleManager _bubbleManager;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _bubbleThrower = FindObjectOfType<BubbleThrower>();

            _inputHandler = FindObjectOfType<InputHandler>();
            _bubbleManager = FindObjectOfType<BubbleManager>();
        }

        protected override void Dispose()
        {
            _bubbleThrower = null;

            _inputHandler = null;
            _bubbleManager = null;
        }

        protected override void SubscribeEvents()
        {
            if (_bubbleThrower == null) return;

            _inputHandler.OnFingerDown += _bubbleThrower.OnFingerDown;
            _inputHandler.OnFinger += _bubbleThrower.OnFinger;
            _inputHandler.OnFingerUp += _bubbleThrower.OnFingerUp;

            _bubbleThrower.BubbleDataRequested += _bubbleManager.OnBubbleDataRequested;
            _bubbleThrower.ThrowBubbleRequested += _bubbleManager.OnThrowBubbleRequested;
        }

        protected override void UnSubscribeEvents()
        {
            if (_bubbleThrower == null) return;

            _inputHandler.OnFingerDown -= _bubbleThrower.OnFingerDown;
            _inputHandler.OnFinger -= _bubbleThrower.OnFinger;
            _inputHandler.OnFingerUp -= _bubbleThrower.OnFingerUp;

            _bubbleThrower.BubbleDataRequested -= _bubbleManager.OnBubbleDataRequested;
            _bubbleThrower.ThrowBubbleRequested -= _bubbleManager.OnThrowBubbleRequested;
        }

        #endregion Functions
    }
}