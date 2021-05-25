using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IValueModel {

        int ValueInt { get; }
        string ValueString { get; set; }
        bool ValueBool{ get; }
        string LocalizedMessage { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}