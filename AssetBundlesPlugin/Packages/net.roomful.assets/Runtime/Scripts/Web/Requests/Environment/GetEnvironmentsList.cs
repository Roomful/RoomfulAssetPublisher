﻿using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	internal class GetEnvironmentsList : GetAssetsList
    {
        public const string RequestUrl = "/api/v0/asset/environment/list";

		public GetEnvironmentsList(int offset, int size, List<string> tags):base(offset, size, tags, RequestUrl) {}
		public GetEnvironmentsList(int offset, int size, string title):base(offset, size, title, RequestUrl) {}

	}
}
