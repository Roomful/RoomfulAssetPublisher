using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IWebSocketPackageTemplate {
        
        string PackUrl { get; }
        Dictionary<string, object> Data { get; }
    }
}