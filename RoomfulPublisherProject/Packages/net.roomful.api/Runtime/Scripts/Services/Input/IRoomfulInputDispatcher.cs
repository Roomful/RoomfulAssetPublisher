using System;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api
{
    //interface for a device input generation of events
    public interface IRoomfulInputDispatcher
    {
        // 1-finger pan (mobile)
        // Left Mouse Drag
        event Action<DragEventData> OnDrag;
        // 2-finger pan (mobile)
        // Ctrl + 2-finger pan (touch pad)
        // Ctrl + Left Mouse Drag
        // Ctrl + Mouse scroll wheel
        event Action<DragEventData> OnAlternativeDrag;
        // 1-finger pan (mobile)
        // Left Mouse Drag
        event Action<DragEventData> OnDragWASD;
        // 1-finger pan (mobile)
        // Left Mouse Drag
        event Action<DragEventData> OnDragArrows;
        event Action<DragEventData> OnClickArrows;
        // 2-finger pan (mobile)
        // Ctrl + 2-finger pan (touch pad)
        // Ctrl + Left Mouse Drag
        // Ctrl + Mouse scroll wheel
        event Action<DragEventData> OnZoom;
        // Rotate gesture (mobile)
        // Alt + 2-finger pan (touch pad)
        // Alt + Left Mouse Drag
        // Alt + Mouse scroll wheel
        event Action<MovementEventData> OnRotation;
        //Time > 0.5s & no movment
        event Action<TouchEventData> OnTouchToEdit;
        //Time < 0.5s & no movment
        event Action<TouchEventData> OnTouchToSelect;
        //touch (mobile)
        //left mouse (standalone)
        event Action<TouchEventData> OnTouch;
        event Action<TouchEventData> OnDoubleTouch;
        event Action<BaseEventData> OnEscapePressed;
        event Action<BaseEventData> OnSpacePressed;
        event Action<BaseEventData> OnEnterPressed;
        event Action<BaseEventData> OnDeletePressed;
        event Action<SwipeEventData> OnSwiped;
        event Action<TurnEventData> OnTurned;
        event Action<BaseEventData> OnActionPressed; 
        event Action<NumberEventData> OnNumberPressed; 

        Vector3 MousePosition { get; }
    }
}
