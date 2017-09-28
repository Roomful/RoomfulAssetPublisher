using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	public class V1_ThumbnailCollector : ICollector {

		public void Run(PropAsset propAsset) {
#if UNITY_EDITOR
			Transform thumbnails =  propAsset.GetLayer ("Thumbnails");

			foreach(Transform tb in thumbnails) {
				var thumbnail = tb.gameObject.AddComponent<PropThumbnail> ();
                

                var info = new SerializedThumbnail();
                info.IsFixedRatio = tb.Find("CanvasRatio") != null;
               

                if (info.IsFixedRatio) {
                    Transform ratio = tb.Find("CanvasRatio");
                    info.XRatio = System.Convert.ToInt32(ratio.GetChild(0).name);
                    info.YRatio = System.Convert.ToInt32(ratio.GetChild(1).name);
                }

                info.IsBoundToResourceIndex = tb.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND) != null;
                if (info.IsBoundToResourceIndex) {
                    Transform obj = tb.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND);
                    info.ResourceIndex =  System.Convert.ToInt32(obj.GetChild(0).name);
                }

                thumbnail.Restore(info);
                tb.gameObject.AddComponent<PropBorder>();
            }


            Transform stands = propAsset.GetLayer("StandSurface");
            foreach (Transform tb in stands) {
                tb.gameObject.AddComponent<RF.AssetBundles.Serialization.SerializedStandMarker>();
                tb.transform.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }

            Transform ignoredGpahics = propAsset.GetLayer("IgnoredGraphics'");
            foreach (Transform tb in ignoredGpahics) {
                tb.gameObject.AddComponent<RF.AssetBundles.Serialization.SerializedBoundsIgnoreMarker>();
                tb.transform.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }



#endif
        }
    }
}
