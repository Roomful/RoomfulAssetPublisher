using System.Collections.Generic;

namespace net.roomful.api.assets
{
    /// <summary>
    /// The Serializable Template API.
    /// </summary>
    public interface ISerializableTemplate : ITemplate
    {
        /// <summary>
        /// Converts template data to the dictionary key / value representation.
        /// </summary>
        /// <returns>New Dictionary instance with asset data inside.</returns>
        Dictionary<string, object> ToDictionary();
    }
}
