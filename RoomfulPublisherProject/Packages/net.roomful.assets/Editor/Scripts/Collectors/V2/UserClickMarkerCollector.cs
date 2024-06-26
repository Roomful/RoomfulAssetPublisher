using net.roomful.assets.serialization;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace net.roomful.assets.editor
{
    internal class UserClickMarkerCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset)
        {
            var markers = asset.gameObject.GetComponentsInChildren<SerializedAnimationMarker>(true);

            foreach (var marker in markers)
            {
                if (marker.TargetClickArea != null)
                {
                    GameObject parentGo;
                    var parentTransform = marker.transform.Find("UserClickMarker");
                    if (parentTransform == null) {
                        parentGo = new GameObject("UserClickMarker");
                        parentGo.transform.localPosition = Vector3.zero;
                    }
                    else {
                        parentGo = parentTransform.gameObject;
                    }
                    parentGo.transform.SetParent(marker.transform);
                    parentGo.AddComponent<UserClickMarker>();
                    var go = PrefabManager.CreatePrefab("Room/PropAnimationButton");
                    go.transform.SetParent(parentGo.transform);
                    go.transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}