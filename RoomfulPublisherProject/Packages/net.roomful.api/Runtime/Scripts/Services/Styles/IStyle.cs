using System.Collections.Generic;

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
        /// Style template Id.
        /// </summary>
        IStyleTemplate Template { get; }
    }
}
