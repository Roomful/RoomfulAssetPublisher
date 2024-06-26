using System.Collections.Generic;
using net.roomful.api.styles;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class StyleKaraokeToggleSetup : StyleComponent
    {
        [SerializeField] List<GameObject> m_Toggles;

        public IReadOnlyCollection<GameObject> Toggles => m_Toggles;
    }
}
