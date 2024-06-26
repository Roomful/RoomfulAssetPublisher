using System.Collections.Generic;

namespace net.roomful.api {
    
    public interface IResourceViewInfo {

        Dictionary<string, object> ToDictionary();
        int StopTime { get; set; }
        bool IsWatched{ get; set; }
    }
}