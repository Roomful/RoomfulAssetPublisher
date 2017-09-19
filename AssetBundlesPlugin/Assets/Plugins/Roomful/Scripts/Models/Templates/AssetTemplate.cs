using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moon.Network.Web;

namespace RF.AssetWizzard {
	
	[Serializable]
	public class AssetTemplate {

		[Param("id")] public string Id = string.Empty;
		[Param("created")] public DateTime Created = DateTime.MinValue;


		public DateTime Updated = DateTime.MinValue;
		public string Title = string.Empty;
		public Placing Placing = Placing.Floor;
		public InvokeTypes InvokeType = InvokeTypes.Default;
		public Resource Icon = null;
		public float MinSize = 0.5f;
		public float MaxSize = 3f;
		public bool CanStack = false;
		public List<ContentType> ContentTypes =  new List<ContentType>();
		public List<string> Tags =  new List<string>();
		public AssetSilhouette Silhouette = null;
		public List<AssetUrl> Urls = new List<AssetUrl>();


		public Vector3 Size =  Vector3.one;

		public AssetTemplate() {
			Icon = new Resource ();
		}

		public AssetTemplate(AssetTemplate origin) {
			Id = origin.Id;
			Created = origin.Created;
			Updated = origin.Updated;
			Title = origin.Title;
			Placing = origin.Placing;
			InvokeType = origin.InvokeType;
			Icon = origin.Icon;
			MinSize = origin.MinSize;
			MaxSize = origin.MaxSize;
		}
			

		public AssetTemplate(string assetData) {
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

			if(Icon != null) {
				OriginalJSON.Add("thumbnail", Icon.ToDictionary());
			}

			OriginalJSON.Add("assetmesh", Silhouette.ToDictionary());



			OriginalJSON.Add("minScale", MinSize);
			OriginalJSON.Add("maxScale", MaxSize);

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


			if(assetData.HasValue("thumbnail")) {
				var resInfo =  new JSONData(assetData.GetValue<Dictionary<string, object>>("thumbnail"));
				Icon = new Resource(resInfo);
			} else {
				Icon = new Resource ();
			}


			if (assetData.HasValue("assetmesh")) {
				var SilhouetteInfo = new JSONData(assetData.GetValue<Dictionary<string, object>>("assetmesh"));
				Silhouette = new AssetSilhouette (SilhouetteInfo);
			}
				

			MinSize = assetData.GetValue<float> ("minScale");
			MaxSize = assetData.GetValue<float> ("maxScale");
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


			if(assetData.HasValue("urls")) {
				var urlsList = assetData.GetValue<Dictionary<string, object>> ("urls");
				if(urlsList != null) {
					foreach(var pair in urlsList) {
						AssetUrl url = new AssetUrl(pair.Key,System.Convert.ToString (pair.Value));
						Urls.Add (url);
					}
				}
			}


		}




		public GUIContent DisaplyContent {

			get {
				GUIContent content = new GUIContent ();


				if(Icon != null && Icon.Thumbnail != null) {
					content.image = Icon.Thumbnail;
				}

				content.text = Title;
				return content;
			}
		}



	}
}