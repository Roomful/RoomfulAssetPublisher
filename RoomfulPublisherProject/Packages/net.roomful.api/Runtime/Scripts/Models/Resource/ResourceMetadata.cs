using System;
using System.Collections.Generic;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api {

	public class ResourceMetadata {

        public static readonly List<string> IMAGE_TYPES = new List<string> { MimeTypePng, MimeTypeJpg, "image/jpeg", "image/gif", "image/bmp", "facebook#photo", "imdb#poster", "pinterest#image", "roomful#url", "amazon#movie", "amazon#mp3", "amazon#music", "amazon#product" };
        public static readonly List<string> AUDIO_TYPES = new List<string> { MimeTypeWav, "audio/mpeg", "audio/ogg", "audio/x-wav", "audio/webm", "audio/mp3", "audio/unity" };
        public static readonly List<string> VIDEO_TYPES = new List<string> { MimeTypeVideoStream, MimeTypeYoutube, "video/mp4", "video/avi", "video/mpeg", "video/ogg", "video/quicktime", "video/webm"  };
        public static readonly List<string> BOOK_TYPES =  new List<string> { "amazon#book", "application/epub+zip", "application/x-fictionbook", "application/pdf" };
        public static readonly List<string> SPECIAL_TYPES =  new List<string> { "directory" };

        public const string MimeTypeJpg = "image/jpg";
		public const string MimeTypePng = "image/png";
		public const string MimeTypeWav = "audio/wav";
		public const string MimeTypeYoutube = "youtube#video";
		public const string MimeTypeSimpleUrl = "roomful#url";
		public const string MimeTypeVideoStream = "application/x-mpegurl";
		public const string MimeTypePdf = "application/pdf";

		public string Name = string.Empty;
		public int Size;
		public string MimeType  = MimeTypePng;
		public string Link = string.Empty;
		public DateTime Date =  new DateTime();
		public ResourceGeo Geo = new ResourceGeo();
		public ResourceOrigin Origin = new ResourceOrigin();
		public ResourceDimentions Dimentions = new ResourceDimentions();

		public ResourceMetadata() {}

		public ResourceMetadata(JSONData metaInfo) {
			ParsePropMetaData(metaInfo);
		}

		public Dictionary<string, object> ToDictionary() {
			var data =  new Dictionary<string, object>();
			data.Add("fileName", Name);
			data.Add("fileSize", Size);
			//data.Add("fileDate", Size);
			data.Add("contentType", MimeType);
			if(!Link.Equals (string.Empty)) {
				data.Add("link", Link);
			}
			data.Add("origin", Origin.ToDictionary());
			data.Add("geolocation", Geo.ToDictionary());
			data.Add("dimensions", Dimentions.ToDictionary());
			return data;
		}

		private void ParsePropMetaData(JSONData metaInfo) {
			Name = metaInfo.GetValue<string>("fileName");
			Size = metaInfo.GetValue<int>("fileSize");
			if(metaInfo.HasValue ("link")) {
				Link =  metaInfo.GetValue<string>("link");
			}
			//Date = metaInfo.GetValue<DateTime>("fileDate");
			MimeType = metaInfo.GetValue<string>("contentType");
			var jsonOriginInfo = new JSONData(metaInfo.GetValue<Dictionary<string, object>>("origin"));
			Origin =  new ResourceOrigin(jsonOriginInfo);
			var jsonGeoInfo = new JSONData(metaInfo.GetValue<Dictionary<string, object>>("geolocation"));
			Geo =  new ResourceGeo(jsonGeoInfo);
			if(metaInfo.HasValue("dimensions")) {
				var jsonDimensionsInfo = new JSONData(metaInfo.GetValue<Dictionary<string, object>>("dimensions"));
				Dimentions =  new ResourceDimentions(jsonDimensionsInfo);
			}
			if(MimeType.Equals(MimeTypeYoutube)) {
				Dimentions.SetSize (16, 9);
			}
		}

		public bool IsVideoMimeTypeStream => MimeType.Equals(MimeTypeVideoStream);
	}
}
