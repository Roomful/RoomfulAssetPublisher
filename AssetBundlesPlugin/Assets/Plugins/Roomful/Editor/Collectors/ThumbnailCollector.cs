using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetWizzard;

namespace RF.AssetBundles {
	public class ThumbnailCollector : ICollector {

		public void Run(PropAsset propAsset) {
#if UNITY_EDITOR
			Transform thumbnails =  propAsset.GetLayer (HierarchyLayers.Thumbnails);

			foreach(Transform tb in thumbnails) {
				PropThumbnail thumbnail = tb.gameObject.AddComponent<PropThumbnail> ();
				FixShaders (thumbnail.Border);
				FixShaders (thumbnail.Corner);
			}

#endif
		}

		private void FixShaders(GameObject obj) {
			if (obj == null) {
				return;
			}

			var renderers = obj.GetComponentsInChildren<Renderer> ();

			foreach (Renderer r in renderers) {
				foreach(Material m in r.sharedMaterials) {
					if(m == null) { continue; }
					if (m.shader == null) { continue; }


					var shaderName = m.shader.name;
					var newShader = Shader.Find(shaderName);
					if(newShader != null){
						m.shader = newShader;
					} else {
						Debug.LogWarning("unable to refresh shader: "+shaderName+" in material "+m.name);
					}
				}
			}
		}
	}
}
