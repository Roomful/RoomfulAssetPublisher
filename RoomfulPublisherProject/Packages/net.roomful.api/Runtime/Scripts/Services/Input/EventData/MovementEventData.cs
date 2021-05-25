// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public class MovementEventData : AbstractInputEventData
    {
        public float DeltaMovement { get; protected set; }

        internal void Init(InputEventPhase phase, float delta) {
            base.Init(phase);
            DeltaMovement = delta;
        }

        private static readonly MovementEventDataPool s_pool = new MovementEventDataPool();

        public static MovementEventDataPool.PooledObject GetPooled(InputEventPhase phase, float delta) {
            return s_pool.Get(phase, delta);
        }
    }

    public class MovementEventDataPool : EventsPool<MovementEventData>
    {
        public PooledObject Get(InputEventPhase phase, float delta) {
            var item = base.Get();
            item.Init(phase, delta);
            return new PooledObject(item, this);
        }
    }
}
