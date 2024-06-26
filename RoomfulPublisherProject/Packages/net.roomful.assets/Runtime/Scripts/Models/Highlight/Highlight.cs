using System;
using HighlightPlus;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class Highlight : PropComponent, IRecreatableOnLoad
    {
        [Serializable]
        public class TooltipData
        {
            public bool Override;
            
            public bool ShowTooltip;
            public string Message = "Inspect";
            public Color Color = Color.white;
        }

        public TooltipData Tooltip;
        
        public HighlightProfile Profile;
    }
}
