using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace net.roomful.assets {

	[Serializable]
	public class ResourceMetadata {

		public const string MimeTypeJpg = "image/jpeg";
		public const string MimeTypePng = "image/png";
		public const string MimeTypeWav = "audio/wav";
		public const string MimeTypeOgg = "audio/ogg";
		public const string MimeTypeYoutube = "youtube#video";

		public string Name = string.Empty;
		public int Size = 0;
		public string Type  = MimeTypePng;


		public string YoutubeLink = string.Empty;

		public DateTime Date =  new DateTime();

		public ResourceGeo Geo = new ResourceGeo();
		public ResourceOrigin Origin = new ResourceOrigin();



		public ResourceMetadata() {

		}

		public ResourceMetadata(JSONData metaInfo) {
			ParsePropMetaData(metaInfo);
		}

		public Dictionary<string, object> ToDictionary() {
			Dictionary<string, object> data =  new Dictionary<string, object>();

			data.Add("fileName", Name);
			data.Add("fileSize", Size);
			//data.Add("fileDate", Size);
			data.Add("contentType", Type);

			if(!YoutubeLink.Equals (string.Empty)) {
				data.Add("link", YoutubeLink);
			}

			data.Add("origin", Origin.ToDictionary());
			data.Add("geolocation", Geo.ToDictionary());

			return data;
		}

		private void ParsePropMetaData(JSONData metaInfo) {
			Name = metaInfo.GetValue<string>("fileName");
			Size = metaInfo.GetValue<int>("fileSize");

			if(metaInfo.HasValue ("link")) {
				YoutubeLink =  metaInfo.GetValue<string>("link");
			}

			//Date = metaInfo.GetValue<DateTime>("fileDate");
			Type = metaInfo.GetValue<string>("contentType");
			var JSONOriginInfo = new JSONData(metaInfo.GetValue<Dictionary<string, object>>("origin"));
			Origin =  new ResourceOrigin(JSONOriginInfo);
			var JSONGeoInfo = new JSONData(metaInfo.GetValue<Dictionary<string, object>>("geolocation"));
			Geo =  new ResourceGeo(JSONGeoInfo);
		}
	}
}
