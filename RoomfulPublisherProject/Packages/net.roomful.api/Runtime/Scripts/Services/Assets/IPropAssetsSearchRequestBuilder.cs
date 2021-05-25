using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IPropAssetsSearchRequestBuilder
    {
        void SetId(string id);
        void SetPlacing(string placing);
        void SetTitle(string title);
        void SetTags(IReadOnlyList<string> tags);
        void SetContent(IReadOnlyList<string> content);
    }
}
