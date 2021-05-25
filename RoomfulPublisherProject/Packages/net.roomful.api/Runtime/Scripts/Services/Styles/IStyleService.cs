using System;
using System.Collections.Generic;

namespace net.roomful.api.styles
{
    public interface IStyleService
    {
        float LeftRoomLimit { get; }
        float RightRoomLimit { get; }

        event Action<float, float> OnRoomLimitsChanged;

        List<IStylePanel> GetActivePanels();
    }
}
