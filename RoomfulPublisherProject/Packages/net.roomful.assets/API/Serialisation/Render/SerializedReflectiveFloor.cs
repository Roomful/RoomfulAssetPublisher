using System.Collections.Generic;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class SerializedReflectiveFloor : PropComponent, IRecreatableOnLoad
    {
        public float Height;
        public float AlbedoBoost;
        public List<Renderer> Targets;
    }
}