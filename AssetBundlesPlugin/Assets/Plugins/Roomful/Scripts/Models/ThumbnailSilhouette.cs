using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {

	public class ThumbnailSilhouette  {

		public string BorderMeshData = string.Empty;
		public string CornerMeshData = string.Empty;
		public Vector3 Position = Vector3.zero;
		public Vector3 Rotation = Vector3.zero;
		public bool IsFixedRation = false;


		public ThumbnailSilhouette(PropThumbnail thumbnail) {

			BorderMeshData = MeshSerializer.SerializerMesh (thumbnail.Border);
			CornerMeshData = MeshSerializer.SerializerMesh (thumbnail.Corner);
			Position = thumbnail.transform.localPosition;
			Rotation = thumbnail.transform.localRotation.eulerAngles;
			IsFixedRation = thumbnail.IsFixedRatio;
		}

		public ThumbnailSilhouette(JSONData thumbnailInfo) {
			ParseTemplate (thumbnailInfo);
		}



		public Dictionary<string, object> ToDictionary() {
		
			Dictionary<string, object> data = new Dictionary<string, object> ();

			data.Add("border", BorderMeshData);
			data.Add("corner", CornerMeshData);


			Dictionary<string, object> position = new Dictionary<string, object>();
			position.Add("x", Position.x);
			position.Add("y", Position.y);
			position.Add("z", Position.z);

			Dictionary<string, object> rotation = new Dictionary<string, object>();
			rotation.Add("x", Rotation.x);
			rotation.Add("y", Rotation.y);
			rotation.Add("z", Rotation.z);

			data.Add("position", position);
			data.Add("rotation", rotation);

			data.Add("is_fixed_ration", IsFixedRation);

			return data;

		}


		private void ParseTemplate(JSONData thumbnailInfo) {
		
			BorderMeshData = thumbnailInfo.GetValue<string> ("border");
			CornerMeshData = thumbnailInfo.GetValue<string> ("corner");

			IsFixedRation = thumbnailInfo.GetValue<bool> ("is_fixed_ration");


			JSONData MobileGeometryPosition = new JSONData(thumbnailInfo.GetValue<Dictionary<string, object>>("position"));

			Position.x = MobileGeometryPosition.GetValue<float>("x");
			Position.y = MobileGeometryPosition.GetValue<float>("y");
			Position.z = MobileGeometryPosition.GetValue<float>("z");

			JSONData MobileGeometryRotation = new JSONData(thumbnailInfo.GetValue<Dictionary<string, object>>("rotation"));

			Rotation.x = MobileGeometryRotation.GetValue<float>("x");
			Rotation.y = MobileGeometryRotation.GetValue<float>("y");
			Rotation.z = MobileGeometryRotation.GetValue<float>("z");

		}
			
	}

}
