using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	public class ComponentsCollector : ICollector {

		public void Run(PropAsset propAsset) {
			

			foreach (SerializedThumbnail thumbnail in propAsset.gameObject.GetComponentsInChildren<SerializedThumbnail>()) {
				thumbnail.gameObject.AddComponent<PropThumbnail> ();
			}

			foreach (SerializedMeshThumbnail meshThumbnail in propAsset.gameObject.GetComponentsInChildren<SerializedMeshThumbnail>()) {
				meshThumbnail.gameObject.AddComponent<PropMeshThumbnail> ();
			}


			foreach (SerializedFrame frame in propAsset.gameObject.GetComponentsInChildren<SerializedFrame>()) {
				frame.gameObject.AddComponent<PropFrame> ();
				GameObject.DestroyImmediate (frame);

			}


		}
	}
}
