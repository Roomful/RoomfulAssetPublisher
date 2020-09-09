using UnityEngine;

namespace net.roomful.assets.Editor
{
    public class V1_MarkersCollector : BaseCollector
    {
        public override void Run(IAsset asset) {
            PropAsset propAsset;
            if (asset is PropAsset propAsset1) {
                propAsset = propAsset1;
            }
            else {
                return;
            }

            var stands = propAsset.GetLayer(HierarchyLayers.StandSurface);
            foreach (Transform tb in stands) {
                tb.gameObject.AddComponent<serialization.SerializedFloorMarker>();
                tb.transform.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }

            if (stands.childCount == 0) {
                Object.DestroyImmediate(stands.gameObject);
            }
            else {
                stands.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }

            var ignoredGpahics = propAsset.GetLayer(HierarchyLayers.IgnoredGraphics);
            foreach (Transform tb in ignoredGpahics) {
                tb.gameObject.AddComponent<serialization.SerializedBoundsIgnoreMarker>();
                tb.transform.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }

            if (ignoredGpahics.childCount == 0) {
                Object.DestroyImmediate(ignoredGpahics.gameObject);
            }
            else {
                ignoredGpahics.parent = propAsset.GetLayer(HierarchyLayers.Graphics);
            }
        }
    }
}