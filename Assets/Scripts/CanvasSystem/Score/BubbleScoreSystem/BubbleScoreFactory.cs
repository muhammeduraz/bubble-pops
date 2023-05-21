using System;
using UnityEngine;
using Assets.Scripts.FactorySystem;

namespace Assets.Scripts.CanvasSystem.BubbleScoreSystem
{
    public class BubbleScoreFactory : BaseFactory<BubbleScore>
    {
        #region Variables

        private BubbleScore _bubbleScore;

        #endregion Variables

        #region Functions

        public BubbleScoreFactory(BubbleScore bubbleScore)
        {

        }

        public override BubbleScore Manufacture()
        {
            return GameObject.Instantiate(_bubbleScore, null, true);
        }

        #endregion Functions
    }
}