using System;
using net.roomful.api.props;

namespace net.roomful.api.zoom
{
    public interface IZoomViewService
    {
        /// <summary>
        /// Fired when zoom view was open
        /// </summary>
        event Action<ZoomViewContext> OnZoomViewOpen;

        /// <summary>
        /// Fired evert time zoom view context is updated.
        /// Please not that when zoom view is open this event is also fired after <see cref="OnZoomViewOpen"/>
        /// </summary>
        event Action<ZoomViewContext> OnZoomContextUpdated;

        /// <summary>
        /// Event is fired when zoom view is closed.
        /// Event contains last active zoom view context.
        /// </summary>
        event Action<ZoomViewCloseContext> OnZoomViewClosed;

        void RegisterPropLinksProvider(IZoomViewPropLinksProvider provider);
        void UnRegisterPropLinksProvider(IZoomViewPropLinksProvider provider);

        void SetCustomPropAction(IProp prop,  ZoomViewPropAction type, ICustomPropAction action);
        void RemoveCustomPropAction(IProp prop, ZoomViewPropAction type, ICustomPropAction action);

        /// <summary>
        /// Open zoom view in specified context
        /// </summary>
        void OpenZoomView(ZoomViewContext context);

        /// <summary>
        /// Show native zoom view for a prop.
        /// </summary>
        /// <param name="prop">Target prop.</param>
        void ShowNativeZoomView(IProp prop);

        /// <summary>
        /// Register own custom zoom view controller.
        /// </summary>
        /// <param name="viewController"></param>
        void RegisterView(IZoomViewController viewController);

        /// <summary>
        /// Returns app URL for the specified zoom view url context;
        /// </summary>
        /// <param name="context">zoom view context url representation.</param>
        /// <returns>App url for the provided context.</returns>
        string GetUrlForContext(ZoomViewUrlContext context);

        /// <summary>
        /// Current Zoom View context.
        /// Context is defined is what is showed right now by the zoom view.
        /// </summary>
        ZoomViewContext Context { get; }

        void SkipCameraFocusForTheNextZoomViewUrl();

        /// <summary>
        /// Deactivate zoom view
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Is zoom view active
        /// </summary>
        bool IsActive { get; }
    }
}
