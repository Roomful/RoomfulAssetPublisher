using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IMultiChoiceScale {
        
        int ScaleId { get; set;  }
        int ScaleOwnerId { get; set; }
        string ScaleName { get; set; }
        List<IValueModel> ScaleValues { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}
