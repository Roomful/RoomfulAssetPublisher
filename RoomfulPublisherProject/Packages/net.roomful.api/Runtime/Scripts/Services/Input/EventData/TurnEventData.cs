namespace net.roomful.api
{
    public enum TurnDirection
    {
        Left,
        Right
    }

    public class TurnEventData : BaseEventData
    {
        private static readonly TurnEventDataPool s_pool = new TurnEventDataPool();

        public TurnDirection Direction { get; private set; }

        public void Init(InputEventPhase phase, TurnDirection direction)
        {
            Init(phase);
            Direction = direction;
        }

        public static TurnEventDataPool.PooledObject GetPooled(InputEventPhase phase, TurnDirection direction) {
            return s_pool.Get(phase, direction);
        }
    }

    public class TurnEventDataPool : EventsPool<TurnEventData>
    {
        public PooledObject Get(InputEventPhase phase, TurnDirection direction) {
            var item = base.Get();
            item.Init(phase, direction);
            return new PooledObject(item, this);
        }
    }
}