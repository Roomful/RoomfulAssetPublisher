using System.Collections.Generic;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class KaraokeToggleSetup : PropComponent, IRecreatableOnLoad
    {
        public List<GameObject> Toggles;
    }
}