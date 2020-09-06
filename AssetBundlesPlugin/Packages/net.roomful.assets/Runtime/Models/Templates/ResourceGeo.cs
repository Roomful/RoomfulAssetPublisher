using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace net.roomful.assets {

	[Serializable]
	public class ResourceGeo  {

		public float Latitude = 0f;
		public float Longitude = 0f;


		public ResourceGeo() {

		}

		public ResourceGeo(JSONData info) {
			if(info.HasValue("latitude")) {
				Latitude = info.GetValue<float>("latitude");
			}

			if(info.HasValue("longitude")) {
				Longitude = info.GetValue<float>("longitude");
			}
		}


		public Dictionary<string, object> ToDictionary() {
			Dictionary<string, object> data =  new Dictionary<string, object>();

			data.Add("latitude", Latitude);
			data.Add("longitude", Longitude);

			return data;
		}
	}
}


