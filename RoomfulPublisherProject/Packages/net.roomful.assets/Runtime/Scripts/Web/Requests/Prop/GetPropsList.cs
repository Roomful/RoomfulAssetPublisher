using System.Collections.Generic;

namespace net.roomful.assets
{
    internal class GetPropsList : GetAssetsList
    {
        private const string REQUEST_URL = "/api/v0/asset/list";

        public GetPropsList(int offset, int size, List<string> tags) : base(offset, size, tags, REQUEST_URL) { }
        public GetPropsList(int offset, int size, string title) : base(offset, size, title, REQUEST_URL) { }
    }
}