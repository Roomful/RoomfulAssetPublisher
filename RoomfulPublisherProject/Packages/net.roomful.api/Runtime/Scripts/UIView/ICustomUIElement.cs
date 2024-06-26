using net.roomful.api.props;
using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.ui
{
    /// <summary>
    /// Interface for custom UI element.
    /// </summary>
    public interface ICustomUIElement
    {
        /// <summary>
        /// Sets parent to element
        /// </summary>
        void SetParent(Transform parent);

        /// <summary>
        /// The element must become active or deactivated
        /// </summary>
        void SetupViewForProp(IProp prop);
    }
}
