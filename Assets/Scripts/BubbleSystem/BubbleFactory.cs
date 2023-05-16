using UnityEngine;
using Assets.Scripts.FactorySystem;

namespace Assets.Scripts.BubbleSystem.Factory
{
    public class BubbleFactory : BaseFactory<Bubble>
    {
        #region Variables

        private Bubble _bubblePrefab;

        #endregion Variables

        #region Functions

        public BubbleFactory(Bubble bubblePrefab) : base()
        {
            _bubblePrefab = bubblePrefab;
        }

        public override Bubble Manufacture()
        {
            Bubble bubble = GameObject.Instantiate(_bubblePrefab, null);
            return bubble;
        }

        #endregion Functions
    }
}