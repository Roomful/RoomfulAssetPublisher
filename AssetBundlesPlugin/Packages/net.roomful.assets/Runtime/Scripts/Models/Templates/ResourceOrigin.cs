using System;
using System.Collections.Generic;


namespace net.roomful.assets {


	[Serializable]
	public class ResourceOrigin  {

		public string Type   = string.Empty;
		public string Device = string.Empty;
		public string Path 	 = string.Empty;


		public ResourceOrigin() {
			
		}

		public ResourceOrigin(JSONData info) {
			Type 	= info.GetValue<string>("type");
			Device 	= info.GetValue<string>("device");
			Path 	= info.GetValue<string>("path");
		}


		public Dictionary<string, object> ToDictionary() {
			var data =  new Dictionary<string, object>();


			data.Add("type", Type);
			data.Add("device", Device);
			data.Add("path", Path);

			return data;
		}

	}
}