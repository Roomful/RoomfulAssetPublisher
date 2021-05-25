using System.Collections.Generic;
using UnityEngine;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public class PositionTemplate : IPositionTemplate {
        
        public Vector3 Position { get; set; }

        public PositionTemplate(Vector3 position) {
            Position = position;
        }

        public PositionTemplate(JSONData data) {
            if (data.HasValue("x") && data.HasValue("y") && data.HasValue("z")) {
                Position = new Vector3(data.GetValue<float>("x"),
                                       data.GetValue<float>("y"),
                                       data.GetValue<float>("z"));
            }
        }

        public Dictionary<string, object> ToDictionary() {
            return new Dictionary<string, object> {
                {"x", Position.x},
                {"y", Position.y},
                {"z", Position.z}
            };
        }
    }
}