using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	public class V1_ThumbnailsCollector : ICollector {

		public void Run(PropAsset propAsset) {


			Transform thumbnails =  propAsset.GetLayer ("Thumbnails");
			foreach(Transform tb in thumbnails) {

				var info = tb.gameObject.AddComponent<SerializedThumbnail>();
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


				tb.gameObject.AddComponent<PropThumbnail> ();
                tb.gameObject.AddComponent<PropFrame>();
            }

			if(thumbnails.childCount == 0) {
				GameObject.DestroyImmediate (thumbnails.gameObject);
			} else {
				thumbnails.parent = propAsset.GetLayer (HierarchyLayers.Graphics);
			}


			//Mesh Thumbnails
			foreach (Transform child in propAsset.transform) {
			
				if(child.name.Equals(AssetBundlesSettings.THUMBNAIL_POINTER)) {
					var info = child.gameObject.AddComponent<SerializedMeshThumbnail>();

					info.IsBoundToResourceIndex = child.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND) != null;
					if (info.IsBoundToResourceIndex) {
						Transform obj = child.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND);
						info.ResourceIndex =  System.Convert.ToInt32(obj.GetChild(0).name);
					}

					child.gameObject.AddComponent<PropMeshThumbnail> ();
				}
			}

				
        }
    }
}
