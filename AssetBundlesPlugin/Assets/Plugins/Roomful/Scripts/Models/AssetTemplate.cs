using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {
	
	[Serializable]
	public class AssetTemplate {
		public string Id = string.Empty;
		public DateTime Created = DateTime.MinValue;
		public DateTime Updated = DateTime.MinValue;
		public string Title = string.Empty;
		public Placing Placing = Placing.Floor;
		public InvokeTypes InvokeType = InvokeTypes.None;
		public Texture2D Thumbnail = null;
		public float MinScale = 0.5f;
		public float MaxScale = 2f;
		public bool CanStack = false;
		public List<ContentType> ContentTypes =  new List<ContentType>();
		public List<string> Tags =  new List<string>();

		public string ThumbnailsBase64String = string.Empty;


		public Vector3 Size =  Vector3.one;


		public AssetTemplate() {
			
		}

		public AssetTemplate(AssetTemplate origin) {
			Id = origin.Id;
			Created = origin.Created;
			Updated = origin.Updated;
			Title = origin.Title;
			Placing = origin.Placing;
			InvokeType = origin.InvokeType;
			Thumbnail = origin.Thumbnail;
			MinScale = origin.MinScale;
			MaxScale = origin.MaxScale;
		}
			

		public AssetTemplate(string assetData) {
			Debug.Log (assetData);
			ParseData (new JSONData(assetData));
		}

		public Dictionary<string, object> ToDictionary () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add("id", Id);
			OriginalJSON.Add("created", SA.Common.Util.General.DateTimeToRfc3339(Created));
			OriginalJSON.Add("updated", SA.Common.Util.General.DateTimeToRfc3339(Updated));
			OriginalJSON.Add("title", Title);
			OriginalJSON.Add("placing", Placing.ToString());
			OriginalJSON.Add("invokeType", InvokeType.ToString());

			string thumbnailStr = "";
			if (Thumbnail != null) {
				byte[] bytes = Thumbnail.EncodeToPNG();

				thumbnailStr = System.Convert.ToBase64String (bytes);
			}

			OriginalJSON.Add("thumbnail", thumbnailStr);

			OriginalJSON.Add("minScale", MinScale);
			OriginalJSON.Add("maxScale", MaxScale);

			var sizeData = new Dictionary<string, object> ();
			sizeData.Add ("x", Size.x);
			sizeData.Add ("y", Size.y);
			sizeData.Add ("z", Size.z);
			OriginalJSON.Add ("size", sizeData);

			OriginalJSON.Add ("canStack", CanStack);
			OriginalJSON.Add ("contentType", ContentTypes);
			OriginalJSON.Add ("tags", Tags);
		

			return OriginalJSON;
		}

		public void ParseData(JSONData assetData) {
			Id = assetData.GetValue<string> ("id");
			Created = assetData.GetValue<DateTime> ("created");
			Updated = assetData.GetValue<DateTime> ("updated");
			Title = assetData.GetValue<string> ("title");

			Placing = SA.Common.Util.General.ParseEnum<Placing> (assetData.GetValue<string> ("placing"));
			InvokeType = SA.Common.Util.General.ParseEnum<InvokeTypes> (assetData.GetValue<string> ("invokeType"));

			string thumbnailStr = assetData.GetValue<string> ("thumbnail");

			if (!string.IsNullOrEmpty (thumbnailStr)) {
				ThumbnailsBase64String = thumbnailStr;

				byte[] base64img = System.Convert.FromBase64String (thumbnailStr);
				Thumbnail = new Texture2D (1, 1);
				Thumbnail.LoadImage (base64img);
			}

			MinScale = assetData.GetValue<float> ("minScale");
			MaxScale = assetData.GetValue<float> ("maxScale");
			CanStack  = assetData.GetValue<bool> ("canStack");


			if (assetData.HasValue ("contentType")) {
				List<object> types = assetData.GetValue<List<object>> ("contentType");

				if(types != null) {
					foreach(object type in types) {
						string typeName = System.Convert.ToString (type);
						ContentType ct = SA.Common.Util.General.ParseEnum<ContentType> (typeName);
						ContentTypes.Add (ct);
					}
				}
			}

			var sizeData = new JSONData (assetData.GetValue<Dictionary<string, object>> ("size"));
			Size.x = sizeData.GetValue<float> ("x");
			Size.y = sizeData.GetValue<float> ("y");
			Size.z = sizeData.GetValue<float> ("z");


			if (assetData.HasValue ("tags")) {
				List<object> tags = assetData.GetValue<List<object>> ("tags");

				if(tags != null) {
					foreach (object tag in tags) {
						string tagName = System.Convert.ToString (tag);
						Tags.Add (tagName);
					}
				}
			}

		}

		public void RestoreThumbnail() {
			if (!string.IsNullOrEmpty (ThumbnailsBase64String)) {
				byte[] base64img = System.Convert.FromBase64String (ThumbnailsBase64String);
				Thumbnail = new Texture2D (1, 1);
				Thumbnail.LoadImage (base64img);
			}
		}



		public GUIContent DisaplyContent {

			get {
				GUIContent content = new GUIContent ();
				if(Thumbnail != null) {
					content.image = Thumbnail;
				}

				content.text = Title;
				return content;
			}
		}



	}
}