using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public class TouchEventData : AbstractInputEventData
    {
        public Vector2 TouchLocation { get; private set; }

        internal void Init(InputEventPhase phase, Vector2 touchLocation) {
            base.Init(phase);
            TouchLocation = touchLocation;
        }

        private static readonly TouchEventDataPool s_pool = new TouchEventDataPool();

        public static TouchEventDataPool.PooledObject GetPooled(InputEventPhase phase, Vector2 touchLocation) {
            return s_pool.Get(phase, touchLocation);
        }
    }

    public class TouchEventDataPool : EventsPool<TouchEventData>
    {
        public PooledObject Get(InputEventPhase phase, Vector2 touchLocation) {
            var item = base.Get();
            item.Init(phase, touchLocation);
            return new PooledObject(item, this);
        }
    }
}
