using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class V1_ThumbnailsCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            PropAsset propAsset;
            if (asset is PropAsset propAsset1) {
                propAsset = propAsset1;
            }
            else {
                return;
            }

            var thumbnails = propAsset.GetLayer(HierarchyLayers.Thumbnails);
            foreach (Transform tb in thumbnails) {
                tb.gameObject.AddComponent<PropThumbnail>();
                tb.gameObject.AddComponent<PropStretchedFrame>();
            }

            if (thumbnails.childCount == 0) {
                Object.DestroyImmediate(thumbnails.gameObject);
            }
            else {
                thumbnails.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }

            //Mesh Thumbnails
            var allChilds = propAsset.GetComponentsInChildren<Transform>();
            foreach (var child in allChilds) {
                if (child.name.Equals(AssetBundlesSettings.THUMBNAIL_POINTER)) {
                    var parent = child.parent;
                    parent.gameObject.AddComponent<PropMeshThumbnail>();
                    Object.DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
