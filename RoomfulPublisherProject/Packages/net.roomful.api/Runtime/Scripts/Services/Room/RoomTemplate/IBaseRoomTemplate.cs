using System.Collections.Generic;
using net.roomful.api.assets;
using net.roomful.api.wallet;

namespace net.roomful.api
{
    public interface IBaseRoomTemplate : ITemplate
    {
        /// <summary>
        /// Room Name.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Room subscription status.
        /// </summary>
        SubscriptionStatus SubscriptionStatus { get; set; }

        /// <summary>
        /// Room Assigned Tags
        /// </summary>
        IReadOnlyCollection<string> Tags { get; }

        /// <summary>
        /// Room Owners list.
        /// </summary>
        IReadOnlyList<string> OwnerIds { get; }
    }
}
