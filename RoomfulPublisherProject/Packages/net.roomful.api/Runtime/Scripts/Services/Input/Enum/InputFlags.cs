using System;

namespace net.roomful.api
{
    [Flags]
    public enum InputFlags
    {
        None = 0,
        Drag = 1 << 0,
        AlternativeDrag = 1 << 1,
        DragWASD = 1 << 2,
        DragArrows = 1 << 3,
        Zoom = 1 << 4,
        Rotation = 1 << 5,
        TouchToEdit = 1 << 6,
        TouchToSelect = 1 << 7,
        Touch = 1 << 8,
        DoubleTouch = 1 << 9,
        EscapePressed = 1 << 10,
        SpacePressed = 1 << 11,
        EnterPressed = 1 << 12,
        DeletePressed = 1 << 13,
        Swiped = 1 << 14,
        Turned = 1 << 15,
        ClickArrows = 1 << 16,
        Action = 1 << 17,
        Number = 1 << 18,

        All = Drag | AlternativeDrag | DragWASD | DragArrows |
              Zoom | Rotation | TouchToEdit | TouchToSelect | Touch | DoubleTouch |
              EscapePressed | SpacePressed | EnterPressed | DeletePressed | Swiped |
              Turned | ClickArrows | Action | Number
    }
}