using Assets.Scripts.InputSystem;
using Assets.Scripts.SubscriberSystem;

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

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _bubbleThrower = FindObjectOfType<BubbleThrower>();

            _inputHandler = FindObjectOfType<InputHandler>();
            _bubbleManager = FindObjectOfType<BubbleManager>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _bubbleThrower = null;

            _inputHandler = null;
            _bubbleManager = null;
        }

        protected override void Subscribe()
        {
            if (_bubbleThrower == null) return;

            _inputHandler.OnFingerDown += _bubbleThrower.OnFingerDown;
            _inputHandler.OnFinger += _bubbleThrower.OnFinger;
            _inputHandler.OnFingerUp += _bubbleThrower.OnFingerUp;

            _bubbleThrower.BubbleRequested += _bubbleManager.OnBubbleRequested;
            _bubbleThrower.BubbleDataRequested += _bubbleManager.OnBubbleDataRequested;
        }

        protected override void UnSubscribe()
        {
            if (_bubbleThrower == null) return;

            _inputHandler.OnFingerDown -= _bubbleThrower.OnFingerDown;
            _inputHandler.OnFinger -= _bubbleThrower.OnFinger;
            _inputHandler.OnFingerUp -= _bubbleThrower.OnFingerUp;

            _bubbleThrower.BubbleRequested -= _bubbleManager.OnBubbleRequested;
            _bubbleThrower.BubbleDataRequested -= _bubbleManager.OnBubbleDataRequested;
        }

        #endregion Functions
    }
}