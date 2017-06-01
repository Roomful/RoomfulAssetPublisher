using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;



namespace RF.AssetWizzard {

	[Serializable]
	public class Resource {
		[SerializeField]
		private string _Id = string.Empty;

		[SerializeField]
		private string _Title = string.Empty;

		[SerializeField]
		private string _Description = string.Empty;

		[SerializeField]
		private string _Location = string.Empty;

		[SerializeField]
		private string _Date = string.Empty;

		[SerializeField]
		private string _Category = string.Empty;

		[SerializeField]
		private string _ThumbnailWebURL = string.Empty;

		[SerializeField]
		private DateTime _LastUpdate = DateTime.MinValue;

		[SerializeField]
		public ResourceMetadata _Meta;


		private Texture2D _Thumbnail = null;
		private AudioClip _audioClip = null;




		private static readonly List<string> AudioTypes = new List<string>{"audio/wav", "audio/mpeg", "audio/ogg", "audio/x-wav", "audio/webm", "audio/mp3", "audio/unity" };
		private static readonly List<string> VideoTypes = new List<string> { "video/mp4", "video/avi", "video/mpeg", "video/ogg", "video/quicktime", "video/webm"};
		private static readonly List<string> ImageTypes = new List<string> { "image/png", "image/jpg", "image/jpeg", "image/gif", "image/bmp"};
		private static readonly List<string> BookTypes = new List<string> { "amazon#book", "application/epub+zip", "application/x-fictionbook", "application/pdf" };

		private Dictionary<string, object> _Params = new Dictionary<string, object> ();
		private Dictionary<string, object> _ServerParams = new Dictionary<string, object> ();
		public enum ParamsKeys {
			webLink,
			webLinkDescription,
			audio,
			audioVolume
		}

		private Action<Texture2D> _onThumbnailLoaded;
		private bool _thumbnailIsLoading;

		//--------------------------------------
		//  Initialization
		//--------------------------------------

		public Resource(Resource tpl) {
			UpdateMeta (tpl);
		}

		public Resource() {
			_Meta = new ResourceMetadata ();
		}

		public Resource(ResourceMetadata ResourceMetaData) {
			_Meta = ResourceMetaData;
		}


		public Resource(string  ResourceData) {
			
			JSONData ResourceInfo = new JSONData (ResourceData);
			ParseTemplate (ResourceInfo);
		}

		public Resource(JSONData ResourceInfo) {
			ParseTemplate (ResourceInfo);
		}

		public Resource(string id, ResourceMetadata meta) {
			_Id = id;
			_Meta = meta;
		}

		public void UpdateMeta(Resource tpl) {
			_Id 						= tpl.Id;
			_Title 						= tpl.Title;
			_Description 				= tpl.Description;
			_Location					= tpl.Location;
			_Date 						= tpl.Date;
			_Thumbnail 					= tpl.Thumbnail;
			_Meta 						= tpl.Meta;
			_ThumbnailWebURL 			= tpl.ThumbnailWebURL;
			_LastUpdate 				= tpl.LastUpdate;
			_Params 					= tpl.Params;
			_Category                   = tpl.Category;
		}

		public void SetId(string id) {
			_Id = id;
		}

		public void SetTitle(string title) {
			_Title = title;
		}

		public void SetDescription(string descr) {
			_Description = descr;
		}

		public void SetLocationName(string name) {
			_Location = name;
		}
			

		public void SetAudioClip(AudioClip clip) {
			_audioClip = clip;
		}

		public void SetThumbnailWebURL(string url) {
			_ThumbnailWebURL = url;
		}


		//--------------------------------------
		//  Public Methods
		//--------------------------------------

		public void AddParam(ParamsKeys paramKey, object paramValue) {
			string param = paramKey.ToString ();

			if (_Params.ContainsKey (param)) {
				_Params [param] = paramValue;
			} else {
				_Params.Add (param, paramValue);
			}
		}

		public bool ContainsParam(ParamsKeys paramKey, bool serverParam = false) {

			var param = paramKey.ToString ();
			return serverParam ? _ServerParams.ContainsKey(param) : _Params.ContainsKey (param);
		}

		public T GetParam<T>(ParamsKeys paramKey, bool serverParam = false) {
			var valueByKey = default(T);
			var param = paramKey.ToString ();

			if(serverParam ? _ServerParams.ContainsKey(param) : _Params.ContainsKey(param)) {
				valueByKey =  (T)Convert.ChangeType(serverParam ? _ServerParams[param] : _Params[param], typeof(T));
			}

			return valueByKey;
		}

		public void RefreshTimestamp() {
			_LastUpdate = DateTime.Now;
		}

		public void LoadThumbnail(Action<Texture2D> callback = null) {

			_thumbnailIsLoading = true;

			Network.Request.DownloadIcon loadThumbnail = new RF.AssetWizzard.Network.Request.DownloadIcon (this);

			loadThumbnail.PackageCallbackData = (data) => {

				Texture2D texture = new Texture2D(2, 2);
				texture.LoadImage(data);
				OnThumbnailLoaded(texture);
			};

			loadThumbnail.Send ();
				
		}



		public Dictionary<string, object> ToDictionary() {


			Dictionary<string, object> data = new Dictionary<string, object> {
				{"id", _Id},
				{"title", _Title},
				{"description", _Description},
				{"location", _Location},
				{"date", _Date},
				{"updated", LastUpdateRFC3339}
			};

			if(!_ThumbnailWebURL.Equals (string.Empty)) {
				data.Add("thumbnail", _ThumbnailWebURL);
			}

			data.Add("metadata", _Meta.ToDictionary());

		


			data.Add ("params",  SA.Common.Data.Json.Serialize (_Params));
            return data;
		}





		public string Serialize() {
			return SA.Common.Data.Json.Serialize(ToDictionary());
		}



		public void GetThumbnail(Action<Texture2D> callback) {
			if (Thumbnail != null) {
				callback(Thumbnail);
			} else {
				if (_thumbnailIsLoading) {
					_onThumbnailLoaded += callback;
				} else {
					callback(null);
				}
			}
		}


		//--------------------------------------
		//  Get / Set
		//--------------------------------------

		public DateTime LastUpdate {
			get {
				return _LastUpdate;
			}
		}
		public string LastUpdateRFC3339 {
			get {
				return SA.Common.Util.General.DateTimeToRfc3339(_LastUpdate);
			}
		}

		public string Id {
			get {
				return _Id; 
			}
		}

		public string Title {
			get
			{
				return _Title;
			}
			set {
				_Title = value;
			}
		}

		public string Description {
			get
			{
				return _Description;
			}
			set {
				_Description = value;
			}
		}

		public Texture2D Thumbnail  {
			get {
				if(_Thumbnail == null) {
					if (string.IsNullOrEmpty (Id)) {
						_Thumbnail = new Texture2D (2, 2);
					} else {
						LoadThumbnail ();
					}
				}
				return _Thumbnail;
			}

			set {
				_Thumbnail = value;
			}
		}

		public string Category {
			get { return _Category; }
		}

		public AudioClip AudioClip {
			get { return _audioClip; }
		}

		public string Location {
			get {
				return _Location;
			}
			set {
				_Location = value;
			}
		}

		public string Date {
			get {
				return _Date;
			}
			set {
				_Date = value;
			}
		}

		public Dictionary<string, object> Params {
			get {
				return _Params;
			}
		}
	
		public ContentType Type {
			get
			{
				if (ImageTypes.Contains(_Meta.Type)) {
					return ContentType.Image;
				}

				if (VideoTypes.Contains(_Meta.Type))
				{
					return ContentType.Video;
				}

				if (AudioTypes.Contains(_Meta.Type))
				{
					return ContentType.Audio;
				}

				if (BookTypes.Contains(_Meta.Type))
				{
					return ContentType.Book;
				}
				
				if (_Meta.Type.Equals(ResourceMetadata.MimeTypeYoutube)) {
					return ContentType.YoutubeVideo;
				}

				return ContentType.Undefined;
			}
		}


		public ResourceMetadata Meta {
			get { 
				return _Meta;
			}
		}
			

		public string ThumbnailWebURL {
			get {
				//return "https://graph.facebook.com/100000100676341/picture?type=normal";
				//return "https://scontent.fhen1-1.fna.fbcdn.net/v/t1.0-9/13907015_1386807344665941_1090590386549686538_n.jpg?oh=24609a34e8b795a29fd1075438a70584&oe=599A4FE0";
				//return "https://pp.userapi.com/c637220/v637220726/35935/HKSgKQ8QanY.jpg";
				//return "http://scontent.xx.fbcdn.net/v/t1.0-1/p200x200/15822574_1550248408325125_9123642601447315987_n.jpg?oh=c322ae6c4cd9be91cdab3afba2eb5183&oe=595B5736";
				return _ThumbnailWebURL;
			}
		}
			
		public bool IsEmpty {
			get {
				return Id.Equals (string.Empty);
			}
		}


		//--------------------------------------
		//  Private Methods
		//--------------------------------------

		private void ParseTemplate(JSONData resourceInfo) {

			if (resourceInfo.HasValue("id")) {
				_Id = resourceInfo.GetValue<string>("id");
			}

			if (resourceInfo.HasValue("updated")) {
				_LastUpdate = resourceInfo.GetValue<DateTime>("updated");
			}

			_Title = resourceInfo.GetValue<string>("title");
			_Description = resourceInfo.GetValue<string>("description");

			if (resourceInfo.HasValue("location")) {
				_Location = resourceInfo.GetValue<string>("location");
			}

			if (resourceInfo.HasValue("date")) {
				_Date = resourceInfo.GetValue<string>("date");
			}

			if (resourceInfo.HasValue("category")) {
				_Category = resourceInfo.GetValue<string>("category");
			}

			if (resourceInfo.HasValue("thumbnail")) {
				_ThumbnailWebURL = resourceInfo.GetValue<string>("thumbnail");
			}



			JSONData JSONMetaInfo = new JSONData(resourceInfo.GetValue<Dictionary<string, object>>("metadata"));
			_Meta = new ResourceMetadata(JSONMetaInfo);

			if (resourceInfo.HasValue("params")) {
				string paramsString = resourceInfo.GetValue<string>("params");

				if (!string.IsNullOrEmpty(paramsString)) {
					JSONData paramsData = new JSONData(paramsString);

					foreach (KeyValuePair<string, object> param in paramsData.Data) {
						_Params.Add(param.Key, param.Value);
					}
				}
			}

			if (resourceInfo.HasValue("data")) {
				_ServerParams = resourceInfo.GetValue<Dictionary<string, object>>("data");
			}
		}

		private void OnThumbnailLoaded (Texture2D tex)
		{
			_thumbnailIsLoading = false;
			Thumbnail = tex;

			if (_onThumbnailLoaded != null)
			{
				_onThumbnailLoaded(tex);
				_onThumbnailLoaded = null;
			}
		}
	}
}
