using System.Collections.Generic;
using UnityEngine;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IPositionTemplate {
        Vector3 Position { get; set; }
        Dictionary<string, object> ToDictionary();
    }
}
