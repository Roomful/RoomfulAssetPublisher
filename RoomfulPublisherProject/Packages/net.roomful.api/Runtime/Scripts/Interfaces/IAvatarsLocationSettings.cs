using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
    public interface IAvatarsLocationSettings
    {
        bool IsShow { get; set; }
        AvatarsShowingMode ShowingMode { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}
