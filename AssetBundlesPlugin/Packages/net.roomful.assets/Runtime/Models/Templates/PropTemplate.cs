using System;
using System.Collections.Generic;
using StansAssets.Foundation;
using System.Text;
using UnityEngine;

namespace net.roomful.assets {

	[Serializable]
	public class PropTemplate : Template {

        public const float MIN_ALLOWED_AXIS_SIZE = 0.5f;
        public const float MAX_ALLOWED_AXIS_SIZE = 50f;

	    public List<ContentType> ContentTypes =  new List<ContentType>();
	    public AssetSilhouette Silhouette;
		public Placing Placing = Placing.Floor;
		public InvokeTypes InvokeType = InvokeTypes.Default;
		public bool CanStack;
		public bool AlternativeZoom;
        public bool PedestalInZoomView = true;

		public Vector3 Size =  Vector3.one;
        protected float m_minSize = MIN_ALLOWED_AXIS_SIZE;
        protected float m_maxSize = MAX_ALLOWED_AXIS_SIZE;

        protected List<PropVariant> m_Variants = new List<PropVariant>();
        protected Dictionary<Renderer, PropVariant> m_VariantByRenderer = new Dictionary<Renderer, PropVariant>();

        public PropTemplate():base() {}
        public PropTemplate(string data) : base(data) { }

		public override Dictionary<string, object> ToDictionary () {
            var OriginalJSON = base.ToDictionary();
			OriginalJSON.Add("placing", Placing.ToString());
			OriginalJSON.Add("invokeType", InvokeType.ToString());
			OriginalJSON.Add("assetmesh", Silhouette.ToDictionary());
			OriginalJSON.Add("minScale", m_minSize);
			OriginalJSON.Add("maxScale", m_maxSize);
			var sizeData = new Dictionary<string, object> {{"x", Size.x}, {"y", Size.y}, {"z", Size.z}};
			OriginalJSON.Add ("size", sizeData);
			OriginalJSON.Add ("canStack", CanStack);
			OriginalJSON.Add ("alternativeZoom", AlternativeZoom);
			OriginalJSON.Add ("contentType", ContentTypes);
            OriginalJSON.Add ("pedestalInZoomView", PedestalInZoomView);
			return OriginalJSON;
		}

		public override void ParseData(JSONData assetData) {
            base.ParseData(assetData);
			Placing = EnumUtility.ParseEnum<Placing> (assetData.GetValue<string> ("placing"));
			InvokeType = EnumUtility.ParseEnum<InvokeTypes> (assetData.GetValue<string> ("invokeType"));
			if (assetData.HasValue("assetmesh")) {
				var SilhouetteInfo = new JSONData(assetData.GetValue<Dictionary<string, object>>("assetmesh"));
				Silhouette = new AssetSilhouette (SilhouetteInfo);
			}
            MinSize = assetData.GetValue<float> ("minScale");
            MaxSize = assetData.GetValue<float> ("maxScale");
			CanStack  = assetData.GetValue<bool> ("canStack");
			AlternativeZoom = assetData.GetValue<bool> ("alternativeZoom");
            if (assetData.HasValue("pedestalInZoomView")) {
                PedestalInZoomView = assetData.GetValue<bool>("pedestalInZoomView");
            }
            if (assetData.HasValue ("contentType")) {
				var types = assetData.GetValue<List<object>> ("contentType");
				if(types != null) {
					foreach(var type in types) {
						var typeName = Convert.ToString (type);
						var ct = EnumUtility.ParseEnum<ContentType> (typeName);
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

		public bool ValidateVariantCreate(IEnumerable<GameObject> gameObjects)
        {
			List<Renderer> usedRenderers = new List<Renderer>();
			List<Renderer> renderers = new List<Renderer>();
			foreach (var go in gameObjects)
			{
				Renderer renderer = go.GetComponent<Renderer>();
				if (renderer != null)
				{
					if (HasVariantForRenderer(renderer))
					{
						usedRenderers.Add(renderer);
					}
					else
					{
						renderers.Add(renderer);
					}
				}
			}

			if (usedRenderers.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Can't create Variant for Selected Renderers collection!");
				builder.AppendLine("Renderers:");
				foreach (var r in usedRenderers)
				{
					builder.AppendLine(r.name);
				}
				builder.AppendLine("already in use!");
				Debug.LogWarning(builder.ToString());
				return false;
			}

			if (renderers.Count == 0)
			{
				return false;
			}

			return true;
		}

		public bool TryCreateVariant(IEnumerable<GameObject> gameObjects, out PropVariant variant, string name)
		{
			variant = null;

			List<Renderer> usedRenderers = new List<Renderer>();
			List<Renderer> renderers = new List<Renderer>();
			foreach (var go in gameObjects)
			{
				Renderer renderer = go.GetComponent<Renderer>();
				if (renderer != null)
				{
					if (HasVariantForRenderer(renderer))
					{
						usedRenderers.Add(renderer);
					}
					else
					{
						renderers.Add(renderer);
					}
				}
			}

			if (usedRenderers.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Can't create Variant for Selected Renderers collection!");
				builder.AppendLine("Renderers:");
				foreach (var r in usedRenderers)
				{
					builder.AppendLine(r.name);
				}
				builder.AppendLine("already in use!");
				Debug.LogWarning(builder.ToString());
				return false;
			}

			if (renderers.Count == 0)
			{
				return false;
			}

			variant = new PropVariant(name, renderers);
			foreach (var renderer in renderers)
			{
				m_VariantByRenderer[renderer] = variant;
			}

			return true;
		}

		public bool HasVariantForRenderer(Renderer renderer)
		{
			return m_VariantByRenderer.ContainsKey(renderer);
		}

		public void AddVariant(PropVariant variant)
		{
			m_Variants.Add(variant);
		}

		public void RemoveVariant(PropVariant variant)
		{
			foreach (var renderer in variant.Renderers)
			{
				m_VariantByRenderer.Remove(renderer);
			}
			m_Variants.Remove(variant);
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

        public IEnumerable<PropVariant> Variants
        {
	        get
	        {
		        return m_Variants;
	        }
        }
	}
}