using System.Collections.Generic;

namespace net.roomful.assets
{
    internal class GetEnvironmentsList : GetAssetsList
    {
        private const string REQUEST_URL = "/api/v0/asset/environment/list";

        public GetEnvironmentsList(int offset, int size, List<string> tags) : base(offset, size, tags, REQUEST_URL) { }
        public GetEnvironmentsList(int offset, int size, string title) : base(offset, size, title, REQUEST_URL) { }
    }
}