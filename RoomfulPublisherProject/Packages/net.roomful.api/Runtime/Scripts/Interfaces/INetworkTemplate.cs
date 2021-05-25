using System;
using System.Collections.Generic;

namespace net.roomful.api {
    
    public interface INetworkTemplate : ITemplate {

        DateTime Created { get; }
        DateTime Updated{ get; }
        string FullName { get; }
        string Type { get; }
        string ResourceId { get; }
        string DefaultRoomId { get; }
        string DefaultSubNetworkId { get; set; } 
        Dictionary<string, object> TutorialRecords { get; }
    }
}
