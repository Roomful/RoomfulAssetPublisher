// Copyright Roomful 2013-2020. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public interface IRoomPoint
    {
        string Id { get; }
        string PointName { set; get; }
        Vector3 PointPosition { set; get; }
        Vector3 PointRotation { set; get; }
        void UpdateData(string name, Vector3 position, Vector3 rotation);
        void SetId(string id);
        Dictionary<string, object> ToDictionary();
    }
}
