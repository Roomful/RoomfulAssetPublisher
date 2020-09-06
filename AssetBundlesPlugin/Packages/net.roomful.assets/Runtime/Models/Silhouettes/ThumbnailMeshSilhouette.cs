using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace net.roomful.assets {

	[System.Serializable]
	public class ThumbnailMeshSilhouette
    {

		public string MeshData = string.Empty;
		public Vector3 Position = Vector3.zero;
		public Vector3 Rotation = Vector3.zero;
		public Vector3 Scale = Vector3.one;



		public ThumbnailMeshSilhouette(PropMeshThumbnail thumbnail) {
            MeshData = MeshSerializer.SerializerMesh(thumbnail.gameObject);


			Position = thumbnail.Silhouette.localPosition;
			Scale = thumbnail.Silhouette.localScale;
			Rotation = thumbnail.Silhouette.localRotation.eulerAngles;
		}

		public ThumbnailMeshSilhouette(JSONData thumbnailInfo) {
			ParseTemplate (thumbnailInfo);
		}



		public Dictionary<string, object> ToDictionary() {
		
			Dictionary<string, object> data = new Dictionary<string, object> ();

            data.Add("mesh_data", MeshData);

            Dictionary <string, object> position = new Dictionary<string, object>();
			position.Add("x", Position.x);
			position.Add("y", Position.y);
			position.Add("z", Position.z);

			Dictionary<string, object> rotation = new Dictionary<string, object>();
			rotation.Add("x", Rotation.x);
			rotation.Add("y", Rotation.y);
			rotation.Add("z", Rotation.z);


			Dictionary<string, object> scale = new Dictionary<string, object>();
			scale.Add("x", Scale.x);
			scale.Add("y", Scale.y);
			scale.Add("z", Scale.z);

			data.Add("position", position);
			data.Add("rotation", rotation);
			data.Add("scale", scale);

			return data;

		}


		private void ParseTemplate(JSONData thumbnailInfo) {

		
			if(thumbnailInfo.HasValue("mesh_data")) {
                MeshData = thumbnailInfo.GetValue<string> ("mesh_data");
			}

	
			JSONData MobileGeometryPosition = new JSONData(thumbnailInfo.GetValue<Dictionary<string, object>>("position"));
			if(MobileGeometryPosition.HasValue("x")) {
				Position.x = MobileGeometryPosition.GetValue<float>("x");
				Position.y = MobileGeometryPosition.GetValue<float>("y");
				Position.z = MobileGeometryPosition.GetValue<float>("z");
			}


			JSONData MobileGeometryRotation = new JSONData(thumbnailInfo.GetValue<Dictionary<string, object>>("rotation"));
			if(MobileGeometryRotation.HasValue("x")) {
				Rotation.x = MobileGeometryRotation.GetValue<float>("x");
				Rotation.y = MobileGeometryRotation.GetValue<float>("y");
				Rotation.z = MobileGeometryRotation.GetValue<float>("z");
			}

			if(thumbnailInfo.HasValue("scale")) {
				JSONData MobileGeometryScale = new JSONData(thumbnailInfo.GetValue<Dictionary<string, object>>("scale"));
				if(MobileGeometryScale.HasValue("x")) {
					Scale.x = MobileGeometryScale.GetValue<float>("x");
					Scale.y = MobileGeometryScale.GetValue<float>("y");
					Scale.z = MobileGeometryScale.GetValue<float>("z");
				}
			}

		}
			
	}

}
