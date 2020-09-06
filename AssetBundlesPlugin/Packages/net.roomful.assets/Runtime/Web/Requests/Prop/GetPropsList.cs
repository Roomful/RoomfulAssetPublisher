using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class GetPropsList : GetAssetsList
    {
        public const string RequestUrl = "/api/v0/asset/list";

		public GetPropsList(int offset, int size, List<string> tags):base(offset, size, tags, RequestUrl) {}
		public GetPropsList(int offset, int size, string title):base(offset, size, title, RequestUrl) {}

	}
}
