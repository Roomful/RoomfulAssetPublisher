using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IApplicationRoute
    {
        /// <summary>
        /// Current url without domain name.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// The permission of the current route.
        /// </summary>
        /// <returns></returns>
        RoutePermission Permission { get; }

        /// <summary>
        /// Returns `true` if url contains specified key and `false` otherwise.
        /// </summary>
        /// <param name="key">A url GET param key.</param>
        /// <returns>`true` if url contains specified key and `false` otherwise. </returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Method returns string value of the specified key.
        /// </summary>
        /// <param name="key">A url GET param key.</param>
        /// <returns>A string value of the specified key.</returns>
        string GetValue(string key);

        /// <summary>
        /// Url GET params represented as key/value collection.
        /// </summary>
        IReadOnlyList<KeyValuePair<string, string>> Params { get; }
    }
}
