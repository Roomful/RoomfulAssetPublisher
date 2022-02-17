using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IVideoChatSettingsAutoPromotion {
        
        bool UseCustomSettings { get; set; }
        int AutoPromotionLimit { get; set; }
        bool AutoDemoteOnLimit { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}