using Assets.Scripts.BubbleSystem;
using Assets.Scripts.EnvironmentSystem;

namespace Assets.Scripts.SubscribeSystem
{
    public class FailTriggerSubscriber : BaseSubscriber 
    {
        #region Variables

        private FailTrigger _failTrigger;

        private BubbleManager _bubbleManager;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _failTrigger = FindObjectOfType<FailTrigger>();

            _bubbleManager = FindObjectOfType<BubbleManager>();
        }

        protected override void Dispose()
        {
            _failTrigger = null;

            _bubbleManager = null;
        }

        protected override void SubscribeEvents()
        {
            if (_failTrigger == null) return;

            _bubbleManager.MergeOperationCompleted += _failTrigger.CheckIfFail;
            _bubbleManager.MoveAllBubblesDownCompleted += _failTrigger.CheckIfFail;
        }

        protected override void UnSubscribeEvents()
        {
            if (_failTrigger == null) return;

            _bubbleManager.MergeOperationCompleted -= _failTrigger.CheckIfFail;
            _bubbleManager.MoveAllBubblesDownCompleted -= _failTrigger.CheckIfFail;
        }

        #endregion Functions
    }
}