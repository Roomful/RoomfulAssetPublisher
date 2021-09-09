using System;
using System.Collections.Generic;

namespace net.roomful.api.styles
{
    public interface IStyleService
    {
        float LeftRoomLimit { get; }
        float RightRoomLimit { get; }

        event Action<float, float> OnRoomLimitsChanged;
        event Action<IStyle> OnStyleLoadCompleted;

        IEnumerable<IStylePanel> GetActivePanels();
        IReadOnlyList<IStyle> Styles { get; }

        /// <summary>
        /// Removes style from the room.
        /// </summary>
        /// <param name="styleTemplateId">The Style template id.</param>
        /// <param name="callback">Callback is fired when style remove animation is completed.</param>
        void RemoveStyle(string styleTemplateId, Action callback);


        IStylePanel GetRoomPanelById(string id);
    }
}
