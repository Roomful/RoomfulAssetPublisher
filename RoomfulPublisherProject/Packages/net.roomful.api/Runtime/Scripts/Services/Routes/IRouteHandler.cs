using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    /// <summary>
    /// Instance to handler the route.
    /// </summary>
    public interface IRouteHandler
    {
        /// <summary>
        /// A list if the url keys that is handler byt this route handler.
        /// </summary>
        IEnumerable<string> UrlKeys { get; }

        /// <summary>
        /// Handle a query.
        /// </summary>
        /// <param name="applicationRoute">The route to handle.</param>
        /// <param name="onComplete">Completed callback is fired when handling is completed.</param>
        void Handle(IApplicationRoute applicationRoute, Action<RouteHandleResult> onComplete);

        /// <summary>
        /// Methods is called when all url keys specified in <see cref="UrlKeys"/> were removed from the url.
        /// </summary>
        void OnUrlKeysRemoved();
    }
}
