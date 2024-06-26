using System.Collections.Generic;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    public class DragEventData : MovementEventData
    {
        internal void Init(InputEventPhase phase, Vector2 movement, Vector2 touchLocation, DragEventType dragType = DragEventType.Pointer) {
            Init(phase, 0f);
            Movement = movement;
            DragType = dragType;
            TouchLocation = touchLocation;
            DeltaMovement = Mathf.Abs(movement.x) > Mathf.Abs(movement.y) ? movement.x : movement.y;
        }

        public Vector2 TouchLocation { get; private set; }
        public Vector2 Movement { get; private set; }
        public DragEventType DragType { get; private set; }

        private static readonly DragEventDataPool s_pool = new DragEventDataPool();

        public static DragEventDataPool.PooledObject GetPooled(InputEventPhase phase, Vector2 movement, Vector2 touchLocation, DragEventType dragType = DragEventType.Pointer) {
            return s_pool.Get(phase, movement, touchLocation, dragType);
        }
    }

    public class DragEventDataPool : EventsPool<DragEventData>
    {
        public PooledObject Get(InputEventPhase phase, Vector2 movement, Vector2 touchLocation, DragEventType dragType = DragEventType.Pointer) {
            var item = base.Get();
            item.Init(phase, movement, touchLocation, dragType);
            return new PooledObject(item, this);
        }
    }
}
