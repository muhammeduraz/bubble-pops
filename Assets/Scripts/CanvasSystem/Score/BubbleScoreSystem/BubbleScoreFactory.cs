using System;
using UnityEngine;
using Assets.Scripts.FactorySystem;

namespace Assets.Scripts.CanvasSystem.BubbleScoreSystem
{
    public class BubbleScoreFactory : BaseFactory<BubbleScore>
    {
        #region Variables

        private BubbleScore _bubbleScorePrefab;

        #endregion Variables

        #region Functions

        public BubbleScoreFactory(BubbleScore bubbleScore)
        {
            _bubbleScorePrefab = bubbleScore;
        }

        public override BubbleScore Manufacture()
        {
            return GameObject.Instantiate(_bubbleScorePrefab, null, true);
        }

        #endregion Functions
    }
}