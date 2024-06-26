using System;
using UnityEngine;

namespace net.roomful.api.roomPoints
{
    public interface IRoomPointsService
    {
        event Action<bool> OnExpandContainer;
        (Vector3 position, Vector3 rotation) DefaultRoomPoint { get; }
        void SetParent(RectTransform parent);
        void SetViewActive(bool value, int highLightIndex = -1);
        void GoToDefaultRoomPoint(bool immediate = false);
    }
}

