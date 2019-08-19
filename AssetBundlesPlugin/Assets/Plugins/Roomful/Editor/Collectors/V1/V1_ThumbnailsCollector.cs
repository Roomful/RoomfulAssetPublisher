using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor
{
	public class V1_ThumbnailsCollector : BaseCollector {

		public override void Run(IAsset asset) {

            PropAsset propAsset;
            if (asset is PropAsset) {
                propAsset = (PropAsset)asset;
            } else {
                return;
            }
            var thumbnails =  propAsset.GetLayer ("Thumbnails");
			foreach(Transform tb in thumbnails) {
				var info = tb.gameObject.AddComponent<SerializedThumbnail>();
                info.IsFixedRatio = tb.Find("CanvasRatio") != null;
               if (info.IsFixedRatio) {
                    var ratio = tb.Find("CanvasRatio");
                    info.XRatio = System.Convert.ToInt32(ratio.GetChild(0).name);
                    info.YRatio = System.Convert.ToInt32(ratio.GetChild(1).name);
                }
                info.IsBoundToResourceIndex = tb.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND) != null;
                if (info.IsBoundToResourceIndex) {
                    var obj = tb.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND);
                    info.ResourceIndex =  System.Convert.ToInt32(obj.GetChild(0).name);
                }
				tb.gameObject.AddComponent<PropThumbnail> ();
                tb.gameObject.AddComponent<PropStretchedFrame>();
            }
			if(thumbnails.childCount == 0) {
				GameObject.DestroyImmediate (thumbnails.gameObject);
			} else {
				thumbnails.parent = propAsset.GetLayer (HierarchyLayers.Graphics);
			}
            //Mesh Thumbnails
            var allChilds = propAsset.GetComponentsInChildren<Transform>();
            foreach (var child in allChilds) {
                if (child.name.Equals(AssetBundlesSettings.THUMBNAIL_POINTER)) {
                    var parent = child.parent;
                    var info = parent.gameObject.AddComponent<SerializedMeshThumbnail>();
                    info.IsBoundToResourceIndex = parent.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND) != null;
                    if (info.IsBoundToResourceIndex) {
                        var obj = parent.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND);
                        info.ResourceIndex = System.Convert.ToInt32(obj.GetChild(0).name);
                    }
                    parent.gameObject.AddComponent<PropMeshThumbnail>();
                    GameObject.DestroyImmediate(child.gameObject);
                }
			}
        }
    }
}