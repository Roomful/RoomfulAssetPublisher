using System.Collections.Generic;

namespace net.roomful.api
{
    public class ResourceViewInfo : IResourceViewInfo
    {
        public int StopTime { get; set; }
        public bool IsWatched { get; set; }

        public ResourceViewInfo() { }

        public ResourceViewInfo(JSONData info) {
            if (info.HasValue("stopTime")) {
                StopTime = info.GetValue<int>("stopTime");
            }

            IsWatched = info.GetValue<bool>("isWatched");
        }

        public Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object> {
                { "stopTime", StopTime },
                { "isWatched", IsWatched }
            };
            return data;
        }
    }
}
