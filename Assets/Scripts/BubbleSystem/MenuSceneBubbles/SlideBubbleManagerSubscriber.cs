using UnityEngine;
using Assets.Scripts.SubscribeSystem;

namespace Assets.Scripts.BubbleSystem.MenuSceneBubbles
{
    public class SlideBubbleManagerSubscriber : BaseSubscriber 
    {
        #region Variables

        private SlideBubbleManager _slideBubbleManager;

        private SlideBubbleCreator _slideBubbleCreator;

        #endregion Variables

        #region Functions

        protected override void Initialize()
        {
            _slideBubbleManager = FindObjectOfType<SlideBubbleManager>();
            
            _slideBubbleCreator = FindObjectOfType<SlideBubbleCreator>();
        }

        protected override void Dispose()
        {
            _slideBubbleManager = null;
            _slideBubbleCreator = null;
        }

        protected override void SubscribeEvents()
        {
            if (_slideBubbleManager == null) return;

            _slideBubbleManager.BubbleRequested += _slideBubbleCreator.OnSlideBubbleRequested;
        }

        protected override void UnSubscribeEvents()
        {
            if (_slideBubbleManager == null) return;

            _slideBubbleManager.BubbleRequested -= _slideBubbleCreator.OnSlideBubbleRequested;
        }

        #endregion Functions
    }
}