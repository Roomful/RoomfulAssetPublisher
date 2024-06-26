using System.Collections.Generic;

namespace net.roomful.assets {
	internal class GetStylesList : GetAssetsList
    {
        public const string RequestUrl = "/api/v0/asset/style/list";

		public GetStylesList(int offset, int size, List<string> tags):base(offset, size, tags, RequestUrl) {}
		public GetStylesList(int offset, int size, string title):base(offset, size, title, RequestUrl) {}

	}
}
