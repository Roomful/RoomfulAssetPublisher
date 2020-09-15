﻿using System;
using UnityEngine;
using System.Collections.Generic;
using net.roomful.api;
using SA.Foundation.Time;
using Json = StansAssets.Foundation.Json;

namespace net.roomful.assets {

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
		private string _ThumbnailData = string.Empty;


		[SerializeField]
		private DateTime _LastUpdate = DateTime.MinValue;

		[SerializeField]
		public ResourceMetadata _Meta;


		private Texture2D _Thumbnail = null;
		private AudioClip _audioClip = null;


		private Dictionary<string, object> _Params = new Dictionary<string, object> ();
		private Dictionary<string, object> _ServerParams = new Dictionary<string, object> ();
		public enum ParamsKeys {
			webLink,
			webLinkDescription,
			audio,
			audioVolume
		}

		private Action<Texture2D> _onThumbnailLoaded;

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
			
			var ResourceInfo = new JSONData (ResourceData);
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
			


		//--------------------------------------
		//  Public Methods
		//--------------------------------------

		public void AddParam(ParamsKeys paramKey, object paramValue) {
			var param = paramKey.ToString ();

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


        private bool m_thumbnailLoadStarted = false;
        public void LoadThumbnail(Action<Texture2D> callback = null) {

            if(m_thumbnailLoadStarted) {
                return;
            }

            m_thumbnailLoadStarted = true;

            if (!string.IsNullOrEmpty(_ThumbnailData)) {
				var byteData = Convert.FromBase64String (_ThumbnailData);
				var texture = new Texture2D (2, 2);
				texture.LoadImage (byteData);
				OnThumbnailLoaded (texture);
                m_thumbnailLoadStarted = false;

                return;
			}

			var getAssetUrl = new Network.Request.GetResourceUrl (Id);
			getAssetUrl.PackageCallbackText = assetUrl => {

				var loadThumbnail = new Network.Request.DownloadIcon (assetUrl);
				loadThumbnail.PackageCallbackData = data => {

					var texture = new Texture2D (2, 2);
					texture.LoadImage (data);


					var byteData  = texture.EncodeToPNG();
					_ThumbnailData = Convert.ToBase64String(byteData);

					OnThumbnailLoaded (texture);
                    m_thumbnailLoadStarted = false;

                };

				loadThumbnail.PackageCallbackError = code => {
					FallBackToDefaultLexture();
                    m_thumbnailLoadStarted = false;
                };

				loadThumbnail.Send ();
			};

			getAssetUrl.PackageCallbackError = errorCode => {

				FallBackToDefaultLexture();
                m_thumbnailLoadStarted = false;
            };

			getAssetUrl.Send ();
					
				
		}

		private void FallBackToDefaultLexture() {
			var texture = new Texture2D (32, 32);
			OnThumbnailLoaded (texture);
		}



		public Dictionary<string, object> ToDictionary() {


			var data = new Dictionary<string, object> {
				{"id", _Id},
				{"title", _Title},
				{"description", _Description},
				{"location", _Location},
				{"date", _Date},
				{"updated", LastUpdateRFC3339}
			};



			data.Add("metadata", _Meta.ToDictionary());

		


			data.Add ("params",  Json.Serialize (_Params));
            return data;
		}





		public string Serialize() {
			return Json.Serialize(ToDictionary());
		}
			
	

		//--------------------------------------
		//  Get / Set
		//--------------------------------------

		public DateTime LastUpdate => _LastUpdate;

		public string LastUpdateRFC3339 => SA_Rfc3339_Time.ToRfc3339(_LastUpdate);

		public string Id => _Id;

		public string Title {
			get => _Title;
			set => _Title = value;
		}

		public string Description {
			get => _Description;
			set => _Description = value;
		}

		public Texture2D Thumbnail  {
			get {
				if(_Thumbnail == null) {
					if (string.IsNullOrEmpty (Id)) {
						_Thumbnail = new Texture2D (32, 32);
					} else {
						LoadThumbnail ();
					}
				}
				return _Thumbnail;
			}

			set => _Thumbnail = value;
		}

		public string Category => _Category;

		public AudioClip AudioClip => _audioClip;

		public string Location {
			get => _Location;
			set => _Location = value;
		}

		public string Date {
			get => _Date;
			set => _Date = value;
		}

		public Dictionary<string, object> Params => _Params;

		public ResourceMetadata Meta => _Meta;

		public bool IsEmpty => Id.Equals (string.Empty);

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





			var JSONMetaInfo = new JSONData(resourceInfo.GetValue<Dictionary<string, object>>("metadata"));
			_Meta = new ResourceMetadata(JSONMetaInfo);

			if (resourceInfo.HasValue("params")) {
				var paramsString = resourceInfo.GetValue<string>("params");

				if (!string.IsNullOrEmpty(paramsString)) {
					var paramsData = new JSONData(paramsString);

					foreach (var param in paramsData.Data) {
						_Params.Add(param.Key, param.Value);
					}
				}
			}

			if (resourceInfo.HasValue("data")) {
				_ServerParams = resourceInfo.GetValue<Dictionary<string, object>>("data");
			}
		}

		private void OnThumbnailLoaded (Texture2D tex) {

			Thumbnail = tex;

			if (_onThumbnailLoaded != null) {
				_onThumbnailLoaded(tex);
				_onThumbnailLoaded = null;
			}
		}
	}
}