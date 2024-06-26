using System;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    [Serializable]
    public class TextContent
    {
        public SerializedDataProvider DataProvider = SerializedDataProvider.Prop;

        public int ResourceIndex = 0;
        public SerializedResourceTextContentSource ResourceContentSource = SerializedResourceTextContentSource.Title;
        
        public string Text { get; protected set; } 
    }
}