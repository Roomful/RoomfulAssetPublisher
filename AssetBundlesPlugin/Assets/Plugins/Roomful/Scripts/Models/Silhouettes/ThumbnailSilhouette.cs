using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {

	[System.Serializable]
	public class ThumbnailSilhouette  {

		public string BorderMeshData = string.Empty;
		public string CornerMeshData = string.Empty;
		public Vector3 Position = Vector3.zero;
		public Vector3 Rotation = Vector3.zero;
		public Vector3 Scale = Vector3.one;
		public bool IsFixedRation = false;
		public int RatioX = 1;
		public int RatioY = 1;

        public bool IsBoundToResourceIndex = false;
        public int ResourceIndex = 0;


        public ThumbnailSilhouette(PropThumbnail thumbnail) {

            /*
			if(thumbnail.Border != null && thumbnail.Corner != null) {
				BorderMeshData = MeshSerializer.SerializerMesh (thumbnail.Border);
				CornerMeshData = MeshSerializer.SerializerMesh (thumbnail.Corner);
			}
            */

			Position = thumbnail.transform.localPosition;
			Scale = thumbnail.transform.localScale;
			Rotation = thumbnail.transform.localRotation.eulerAngles;
			IsFixedRation = thumbnail.IsFixedRatio;
			if(IsFixedRation) {
				RatioX = thumbnail.XRatio;
				RatioY = thumbnail.YRatio;
			}


		}

		public ThumbnailSilhouette(JSONData thumbnailInfo) {
			ParseTemplate (thumbnailInfo);
		}



		public Dictionary<string, object> ToDictionary() {
		
			Dictionary<string, object> data = new Dictionary<string, object> ();

			if(!string.IsNullOrEmpty(BorderMeshData) && !string.IsNullOrEmpty(CornerMeshData)) {
				data.Add("border", BorderMeshData);
				data.Add("corner", CornerMeshData);
			}



			Dictionary<string, object> position = new Dictionary<string, object>();
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

			data.Add("is_fixed_ration", IsFixedRation);
			data.Add("x_ration", RatioX);
			data.Add("y_ration", RatioY);


            data.Add("is_bpund_to_resource_index", IsBoundToResourceIndex);
            data.Add("resource_index", ResourceIndex);

            return data;

		}


		private void ParseTemplate(JSONData thumbnailInfo) {

		
			if(thumbnailInfo.HasValue("border")) {
				BorderMeshData = thumbnailInfo.GetValue<string> ("border");
			}

			if(thumbnailInfo.HasValue("corner")) {
				CornerMeshData = thumbnailInfo.GetValue<string> ("corner");
			}

			if(thumbnailInfo.HasValue("is_fixed_ration")) {
				IsFixedRation = thumbnailInfo.GetValue<bool> ("is_fixed_ration");
			}

			if(thumbnailInfo.HasValue("x_ration")) {
				RatioX = thumbnailInfo.GetValue<int> ("x_ration");
			}

			if(thumbnailInfo.HasValue("y_ration")) {
				RatioY = thumbnailInfo.GetValue<int> ("y_ration");
			}

            if (thumbnailInfo.HasValue("is_bound_to_resource_index")) {
                IsBoundToResourceIndex = thumbnailInfo.GetValue<bool>("is_bpund_to_resource_index");
            }

            if (thumbnailInfo.HasValue("resource_index")) {
                ResourceIndex = thumbnailInfo.GetValue<int>("x_ration");
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
