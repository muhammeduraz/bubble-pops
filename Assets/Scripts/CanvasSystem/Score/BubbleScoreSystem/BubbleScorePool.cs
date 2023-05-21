using Assets.Scripts.FactorySystem;
using Assets.Scripts.ProductSystem.Pool;

namespace Assets.Scripts.CanvasSystem.BubbleScoreSystem
{
    public class BubbleScorePool : BasePool<BubbleScore>
    {
        public BubbleScorePool(BaseFactory<BubbleScore> factory) : base(factory)
        {

        }
    }
}