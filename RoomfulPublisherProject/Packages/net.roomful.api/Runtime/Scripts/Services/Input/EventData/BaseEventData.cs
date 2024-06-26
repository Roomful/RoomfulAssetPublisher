// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public class BaseEventData : AbstractInputEventData
    {
        private static readonly BaseEventDataPool s_pool = new BaseEventDataPool();

        public static BaseEventDataPool.PooledObject GetPooled(InputEventPhase phase) {
            return s_pool.Get(phase);
        }
    }

    public class BaseEventDataPool : EventsPool<BaseEventData>
    {
        public PooledObject Get(InputEventPhase phase) {
            var item = base.Get();
            item.Init(phase);
            return new PooledObject(item, this);
        }
    }
}
