using Assets.Scripts.FactorySystem;
using Assets.Scripts.ProductSystem.Pool;

namespace Assets.Scripts.BubbleSystem.Pool
{
    public class BubblePool : BasePool<Bubble>
    {
        public BubblePool(BaseFactory<Bubble> factory) : base(factory)
        {

        }
    }
}