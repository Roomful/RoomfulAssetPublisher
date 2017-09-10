using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {
		
	[System.Serializable]
	public class AssetSilhouette {


		public string MeshData = string.Empty;
		public List<ThumbnailSilhouette> Thumbnails = new List<ThumbnailSilhouette> ();


		public AssetSilhouette(JSONData silhouettelInfo) {
			ParseTemplate (silhouettelInfo);
		}


		public AssetSilhouette(PropAsset asset) {


			MeshData = MeshSerializer.SerializerMesh (asset.GetLayer(HierarchyLayers.Silhouette).gameObject);

			PropThumbnail[] thumbnails = asset.GetComponentsInChildren<PropThumbnail> ();
		
			foreach(PropThumbnail thumbnail in thumbnails) {
				var t = new ThumbnailSilhouette (thumbnail);
				Thumbnails.Add (t);
			}
			
		}


		public Dictionary<string, object> ToDictionary() {


		
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data.Add("mesh_data", MeshData);

			List<Dictionary<string, object>> thumbnails = new List<Dictionary<string, object>>();
			foreach (ThumbnailSilhouette thumbnail in Thumbnails) {
				thumbnails.Add(thumbnail.ToDictionary());
			}

			data.Add("thumbnails", thumbnails);


			return data;
		}


		private void ParseTemplate(JSONData silhouettelInfo) {

			MeshData = silhouettelInfo.GetValue<string> ("mesh_data");

			if (silhouettelInfo.HasValue("thumbnails")) {
				List<object> thumbnailsList = silhouettelInfo.GetValue<List<object>>("thumbnails");
				foreach (object  thumbnail in thumbnailsList) {
					JSONData ThumbnailInfo = new JSONData(thumbnail);

					if(ThumbnailInfo.Data == null) {
						continue;
					}

					var t = new ThumbnailSilhouette (ThumbnailInfo);
					Thumbnails.Add (t);

				}
			}
		}


	}


}