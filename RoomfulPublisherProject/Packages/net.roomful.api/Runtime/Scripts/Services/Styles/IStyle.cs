using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.styles
{
    /// <summary>
    /// Built room style representation.
    /// </summary>
    public interface IStyle
    {
        /// <summary>
        /// Currently active panels in the style.
        /// </summary>
        IReadOnlyList<IStylePanel> ActivePanels { get; }


        /// <summary>
        /// Currently active panels in the style.
        /// </summary>
        IReadOnlyList<IStylePanel> AvailablePanels { get; }

        /// <summary>
        /// Style template Id.
        /// </summary>
        IStyleTemplate Template { get; }

        Vector3 LeftBorderVertex { get; }
        Vector3 RightBorderVertex { get; }
    }
}
