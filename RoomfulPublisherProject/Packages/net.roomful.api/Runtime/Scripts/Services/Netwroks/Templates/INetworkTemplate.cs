using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.networks {

    public interface INetworkTemplate : ITemplate {

        /// <summary>
        /// Network created date.
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// Network updated date.
        /// </summary>
        DateTime Updated{ get; }

        /// <summary>
        /// Full network name.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Network type string
        /// </summary>
        string Type { get; }
        string ResourceId { get; }
        string DefaultSubNetworkId { get;}
        IReadOnlyDictionary<string, object> TutorialRecords { get; }
        INetworkDefaults NetworkDefaults { get; }

        void GetThumbnail(Action<Texture2D> callback);
    }
}
