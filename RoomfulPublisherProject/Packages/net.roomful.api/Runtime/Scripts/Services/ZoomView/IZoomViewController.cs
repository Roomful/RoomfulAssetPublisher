// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.zoom
{
    /// <summary>
    /// Interface for custom zoom view UI.
    /// </summary>
    public interface IZoomViewController
    {
        /// <summary>
        /// Called once when view is registered.
        /// </summary>
        /// <param name="zoomViewUI">Zoom view UI instance.</param>
        void Init(IZoomViewUI zoomViewUI);

        /// <summary>
        /// Setup UI for prop is called when zoom view is moved to another context,
        /// or when view has to be force refreshed.
        /// For Example when we have received the prop updated.
        /// </summary>
        void SetupView(ZoomViewContext zoomViewContext);

        /// <summary>
        /// Deactivate UI is called on zoom view hide.
        /// </summary>
        void Deactivate();
    }
}
