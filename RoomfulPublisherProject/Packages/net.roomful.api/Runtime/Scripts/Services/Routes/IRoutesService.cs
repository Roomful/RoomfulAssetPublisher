using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    /// <summary>
    /// Application routes service
    /// </summary>
    public interface IRoutesService
    {
        /// <summary>
        /// Current Application Route
        /// </summary>
        IApplicationRoute CurrentRoute { get; }

        /// <summary>
        /// Returns `true` if route service currently procession some route.
        /// </summary>
        bool IsProcessing { get; }

        /// <summary>
        /// The default app url.
        /// App default route will be used as the first launch route is no route is provided.
        /// Also if we are unable to perform <see cref="GetPreviousUrl"/>
        /// </summary>
        string ApplicationDefaultUrl { get; }

        /// <summary>
        /// Event is fired when application routes is updated.
        /// Also sometimes route can be the same, but event is still fired,
        /// since setting this route will result in some actions on the app side.
        ///
        /// In other words:
        /// Route can be the same. It not necessary have changed.
        /// But setting this route result in some actions performed.
        /// </summary>
        event Action<RoutesProcessResult> OnApplicationRouteUpdated;

        /// <summary>
        /// Returns previous url if exists. Otherwise <see cref="string.Empty"/> is returned.
        /// </summary>
        /// <returns>Previous url if exists.</returns>
        string GetPreviousUrl();

        /// <summary>
        /// App default route will be used as the first launch route is no route is provided.
        /// Also if we are unable to perform <see cref="GetPreviousUrl"/>
        /// </summary>
        /// <param name="url"></param>
        void RegisterAppDefaultUrl(string url);

        /// <summary>
        /// Switches context to the specified route.
        /// The route is always formatted as key / value pair
        /// examples:
        /// `/room/fgg3p44759xp42/prop/586w0b4zvm8bw0`
        /// `/lobby/activity`
        ///
        /// The only case when key can be without value if this is the last key in the url
        /// examples:
        /// `/room/fgg3p44759xp42/prop/586w0b4zvm8bw0/manage`
        /// `/lobby`
        /// </summary>
        /// <param name="route">new route value</param>
        /// <param name="callback">
        /// Callback is fired when route is fully handled.
        /// Please note that callback may never be fired.
        /// For example,
        /// Currently processing route -> 'room/1'
        /// You setting new route `lobby`, so your route is pending.
        /// Then before 'room/1' route is fully processed, somebody sets new url, let's say `room/2`
        /// Now your route is no longer pending, so it will never be processed and as the result you won't receive a callback.
        /// ALWAYS KEEP THAT IN MIND.
        ///
        /// TODO If this approach with skipping pending url will give us pain. we should consider processing al the routes in a queue ask Kostya or Stan.
        /// </param>
        void SetRoute(string route, Action<RoutesProcessResult> callback = null);

        void ClearRoute();

        void RemoveKeys(IEnumerable<string> keysToRemove, Action<RoutesProcessResult> callback = null);
        void AppendUrl(IEnumerable<KeyValuePair<string, string>> kvpToAdd, Action<RoutesProcessResult> callback = null);

        void ReplaceKeyValue(string key, string value, Action<RoutesProcessResult> callback = null);
        void AddOrReplaceKeyValue(string key, string value, Action<RoutesProcessResult> callback = null);

        /// <summary>
        /// Registers new route handler.
        /// </summary>
        /// <param name="handler"></param>
        void Register(IRouteHandler handler);

        /// <summary>
        /// Goes back to previous route.
        /// Will return <see cref="RoutesProcessStatus.Failed"/> if nothing to go back to.
        /// </summary>
        /// <param name="callback">Callback is fired when route is fully handled.</param>
        void ReturnToPrevUrl(Action<RoutesProcessResult> callback = null);

        /// <summary>
        /// Removes perviest url from the history.
        /// </summary>
        void DoNotAddCurrentUrlToHistory();
        void CancelRemovingCurrentUrlFromHistory();
        void RemoveLastRoute();
        void ClearHistory();

        /// <summary>
        /// Open custom url link.
        /// Link processor will be chosen based on registered schemes. via
        /// The 'roomful://' scheme is default scheme for process internal roomful links.
        /// For example calling 'OpenUrl' with the link 'roomful://room/1/prop/2' is the same as
        /// calling <see cref="SetRoute"/> for 'room/1/prop/2'. 
        /// </summary>
        /// <param name="urlLink"></param>
        void OpenUrl(UrlLink urlLink);

        /// <summary>
        /// Register custom route processor.
        /// </summary>
        /// <param name="scheme">Routes started with this scheme will be transferred to your custom processor.</param>
        /// <param name="processor">Route Processor that will correspond to the provided scheme.</param>
        void RegisterScheme(string scheme, IRouteProcessor processor);
    }
}