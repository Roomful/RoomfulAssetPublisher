using System;
using System.Collections.Generic;
using StansAssets.Foundation;
using System.Text;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets {
	
	public class PropTemplate : Template {

        public const float MIN_ALLOWED_AXIS_SIZE = 0.5f;
        public const float MAX_ALLOWED_AXIS_SIZE = 50f;

	    public List<ContentType> ContentTypes =  new List<ContentType>();
	    public PlacingType Placing = PlacingType.Floor;
		public InvokeTypes InvokeType = InvokeTypes.Default;
		public bool CanStack;
		public bool AlternativeZoom;
        public bool PedestalInZoomView = true;

		public Vector3 Size =  Vector3.one;
		private float m_minSize = MIN_ALLOWED_AXIS_SIZE;
		private float m_maxSize = MAX_ALLOWED_AXIS_SIZE;

		private readonly List<PropVariant> m_variants = new List<PropVariant>();
		private readonly Dictionary<Renderer, PropVariant> m_variantByRenderer = new Dictionary<Renderer, PropVariant>();

		public override Dictionary<string, object> ToDictionary () {
            var originalJSON = base.ToDictionary();
			originalJSON.Add("placing", Placing.ToString());
			originalJSON.Add("invokeType", InvokeType.ToString());
			originalJSON.Add("minScale", m_minSize);
			originalJSON.Add("maxScale", m_maxSize);
			var sizeData = new Dictionary<string, object> {{"x", Size.x}, {"y", Size.y}, {"z", Size.z}};
			originalJSON.Add ("size", sizeData);
			originalJSON.Add ("canStack", CanStack);
			originalJSON.Add ("alternativeZoom", AlternativeZoom);
			originalJSON.Add ("contentType", ContentTypes);
            originalJSON.Add ("pedestalInZoomView", PedestalInZoomView);
			return originalJSON;
		}

		public override void ParseData(JSONData assetData) {
            base.ParseData(assetData);
			Placing = EnumUtility.ParseEnum<PlacingType> (assetData.GetValue<string> ("placing"));
			InvokeType = EnumUtility.ParseEnum<InvokeTypes> (assetData.GetValue<string> ("invokeType"));
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
			var usedRenderers = new List<Renderer>();
			var renderers = new List<Renderer>();
			foreach (var go in gameObjects)
			{
				var renderer = go.GetComponent<Renderer>();
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
				var builder = new StringBuilder();
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

			var usedRenderers = new List<Renderer>();
			var renderers = new List<Renderer>();
			foreach (var go in gameObjects)
			{
				var renderer = go.GetComponent<Renderer>();
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
				var builder = new StringBuilder();
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
				m_variantByRenderer[renderer] = variant;
			}

			return true;
		}

		public bool HasVariantForRenderer(Renderer renderer)
		{
			return m_variantByRenderer.ContainsKey(renderer);
		}

		public void AddVariant(PropVariant variant)
		{
			m_variants.Add(variant);
		}

		public void RemoveVariant(PropVariant variant)
		{
			foreach (var renderer in variant.Renderers)
			{
				m_variantByRenderer.Remove(renderer);
			}
			m_variants.Remove(variant);
		}

        public float MinSize {
            get => m_minSize;
            set {
                value = Math.Max(MIN_ALLOWED_AXIS_SIZE, value);
                m_minSize = value;
            }
        }

        public float MaxSize {
            get => m_maxSize;
            set {
                value = Math.Min(MAX_ALLOWED_AXIS_SIZE, value);
                m_maxSize = value;
            }
        }

        public IEnumerable<PropVariant> Variants => m_variants;
	}
}