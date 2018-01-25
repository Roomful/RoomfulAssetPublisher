﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {
	
	[Serializable]
	public class PropTemplate : Template
    {
        public const float MIN_ALLOWED_AXIS_SIZE = 0.5f;
        public const float MAX_ALLOWED_AXIS_SIZE = 4f;


		public Placing Placing = Placing.Floor;
		public InvokeTypes InvokeType = InvokeTypes.Default;

		public bool CanStack = false;
        public bool PedestalInZoomView = true;
        public List<ContentType> ContentTypes =  new List<ContentType>();
		public AssetSilhouette Silhouette = null;

		public Vector3 Size =  Vector3.one;
        protected float m_minSize = MIN_ALLOWED_AXIS_SIZE;
        protected float m_maxSize = MAX_ALLOWED_AXIS_SIZE;



        public PropTemplate():base() {}
        public PropTemplate(string data) : base(data) { }




		public override Dictionary<string, object> ToDictionary () {
            Dictionary<string, object> OriginalJSON = base.ToDictionary();
			OriginalJSON.Add("placing", Placing.ToString());
			OriginalJSON.Add("invokeType", InvokeType.ToString());

			OriginalJSON.Add("assetmesh", Silhouette.ToDictionary());

			OriginalJSON.Add("minScale", m_minSize);
			OriginalJSON.Add("maxScale", m_maxSize);

			var sizeData = new Dictionary<string, object> ();
			sizeData.Add ("x", Size.x);
			sizeData.Add ("y", Size.y);
			sizeData.Add ("z", Size.z);
			OriginalJSON.Add ("size", sizeData);

			OriginalJSON.Add ("canStack", CanStack);
			OriginalJSON.Add ("contentType", ContentTypes);
            OriginalJSON.Add ("pedestalInZoomView", PedestalInZoomView);
        

			return OriginalJSON;
		}



		public override void ParseData(JSONData assetData) {

            base.ParseData(assetData);

			Placing = SA.Common.Util.General.ParseEnum<Placing> (assetData.GetValue<string> ("placing"));
			InvokeType = SA.Common.Util.General.ParseEnum<InvokeTypes> (assetData.GetValue<string> ("invokeType"));

			if (assetData.HasValue("assetmesh")) {
				var SilhouetteInfo = new JSONData(assetData.GetValue<Dictionary<string, object>>("assetmesh"));
				Silhouette = new AssetSilhouette (SilhouetteInfo);
			}

            MinSize = assetData.GetValue<float> ("minScale");
            MaxSize = assetData.GetValue<float> ("maxScale");
			CanStack  = assetData.GetValue<bool> ("canStack");

            if (assetData.HasValue("pedestalInZoomView")) {
                PedestalInZoomView = assetData.GetValue<bool>("pedestalInZoomView");
            }


            if (assetData.HasValue ("contentType")) {
				List<object> types = assetData.GetValue<List<object>> ("contentType");

				if(types != null) {
					foreach(object type in types) {
						string typeName = System.Convert.ToString (type);
						ContentType ct = SA.Common.Util.General.ParseEnum<ContentType> (typeName);

                        if(!ContentTypes.Contains(ct)) {
                            ContentTypes.Add(ct);
                        }
						
					}
				}
			}

			var sizeData = new JSONData (assetData.GetValue<Dictionary<string, object>> ("size"));
			Size.x = sizeData.GetValue<float> ("x");
			Size.y = sizeData.GetValue<float> ("y");
			Size.z = sizeData.GetValue<float> ("z");
		}


        public float MinSize {
            get {
                return m_minSize;
            }

            set {
                value = Math.Max(MIN_ALLOWED_AXIS_SIZE, value);
                m_minSize = value;
            }
        }

        public float MaxSize {
            get {
                return m_maxSize;
            }

            set {
                value = Math.Min(MAX_ALLOWED_AXIS_SIZE, value);
                m_maxSize = value;
            }
        }

    }
}