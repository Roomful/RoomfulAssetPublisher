using System.Collections.Generic;

namespace net.roomful.api {
    public interface IShareScreenIdentity {
        string UserId { get; }

        /// <summary>
        /// represents single user connection to videochat
        /// </summary>
        string Identity { get; }

        /// <summary>
        /// numeric identity for agora; empty for twilio
        /// </summary>
        int Uid { get; }

        Dictionary<string, object> ToDictionary();
    }
}