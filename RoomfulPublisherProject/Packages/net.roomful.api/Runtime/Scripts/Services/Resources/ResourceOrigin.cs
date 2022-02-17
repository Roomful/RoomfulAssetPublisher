using System.Collections.Generic;

namespace net.roomful.api
{
	public class ResourceOrigin  {

		public string Type   = string.Empty;
		public string Device = string.Empty;
		public string Path 	 = string.Empty;


		public ResourceOrigin() {
		
		}

		public ResourceOrigin(net.roomful.api.JSONData info) {
			Type 	= info.GetValue<string>("type");
			Device 	= info.GetValue<string>("device");
			Path 	= info.GetValue<string>("path");
		}


		public Dictionary<string, object> ToDictionary() {
			Dictionary<string, object> data =  new Dictionary<string, object>();


			data.Add("type", Type);
			data.Add("device", Device);
			data.Add("path", Path);

			return data;
		}
	}
}

