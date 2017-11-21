using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor
{
	public class V1_MarkersCollector : ICollector {

		public void Run(IAsset asset) {

            PropAsset propAsset = null;
            if (asset is PropAsset) {
                propAsset =  (PropAsset) asset;
            } else {
                return;
            }

            Transform stands = propAsset.GetLayer("StandSurface");
            foreach (Transform tb in stands) {
                tb.gameObject.AddComponent<RF.AssetBundles.Serialization.SerializedFloorMarker>();
                tb.transform.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }

			if(stands.childCount == 0) {
				GameObject.DestroyImmediate (stands.gameObject);
			} else {
				stands.parent = propAsset.GetLayer (HierarchyLayers.Graphics);
			}


            Transform ignoredGpahics = propAsset.GetLayer("IgnoredGraphics");
            foreach (Transform tb in ignoredGpahics) {
                tb.gameObject.AddComponent<RF.AssetBundles.Serialization.SerializedBoundsIgnoreMarker>();
                tb.transform.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }


			if(ignoredGpahics.childCount == 0) {
				GameObject.DestroyImmediate (ignoredGpahics.gameObject);
			} else {
				ignoredGpahics.parent = propAsset.GetLayer (HierarchyLayers.Graphics);
			}
				
        }
    }
}
