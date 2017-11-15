using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {
	
	[Serializable]
	public class AssetTemplate : Template
    {
		public Placing Placing = Placing.Floor;
		public InvokeTypes InvokeType = InvokeTypes.Default;
		
		public float MinSize = 0.5f;
		public float MaxSize = 3f;
		public bool CanStack = false;
		public List<ContentType> ContentTypes =  new List<ContentType>();
		public AssetSilhouette Silhouette = null;


		public Vector3 Size =  Vector3.one;

		public AssetTemplate():base() {
			
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

		public override Dictionary<string, object> ToDictionary () {
            Dictionary<string, object> OriginalJSON = base.ToDictionary();


			
			OriginalJSON.Add("placing", Placing.ToString());
			OriginalJSON.Add("invokeType", InvokeType.ToString());

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
		

			return OriginalJSON;
		}

		public override void ParseData(JSONData assetData) {

			Placing = SA.Common.Util.General.ParseEnum<Placing> (assetData.GetValue<string> ("placing"));
			InvokeType = SA.Common.Util.General.ParseEnum<InvokeTypes> (assetData.GetValue<string> ("invokeType"));

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
		}



	}
}