using Assets.Scripts.FactorySystem;
using Assets.Scripts.ProductSystem.Pool;

namespace Assets.Scripts.CanvasSystem.Score.BubbleScoreSystem
{
    public class BubbleScorePool : BasePool<BubbleScore>
    {
        public BubbleScorePool(BaseFactory<BubbleScore> factory) : base(factory)
        {

        }
    }
}