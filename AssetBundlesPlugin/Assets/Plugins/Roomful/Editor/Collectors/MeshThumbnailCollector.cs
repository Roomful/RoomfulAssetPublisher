using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetWizzard;

namespace RF.AssetBundles {
	public class MeshThumbnailCollector : ICollector {

		public void Run(PropAsset propAsset) {
#if UNITY_EDITOR
			List<Transform> pointers = new List<Transform> ();
			Transform[] children = propAsset.gameObject.GetComponentsInChildren<Transform>();

			for (int i = 0; i < children.Length; i++) {
				Transform child = children[i];

				if (child.name.Equals(AssetBundlesSettings.THUMBNAIL_POINTER)) {
					child.parent.gameObject.AddComponent<PropMeshThumbnail> ().Update();
					pointers.Add (child);
				}
			}

			foreach(Transform t in pointers) {
				GameObject.DestroyImmediate (t.gameObject);
			}

#endif
		}
	}
}
