// Copyright Roomful 2013-2020. All rights reserved.

using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IRoomMetadata
    {
        string Name { get; set; }
        string Description { get; set; }
        List<string> Tags { get; set; }
        string Location { get; set; }
        RoomPrivacyMode Privacy { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}
