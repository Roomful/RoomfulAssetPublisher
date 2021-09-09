using RF.AssetBundles;
using UnityEngine;

namespace net.roomful
{
    public static class GameObjectExtensions
    {
        public static Bounds GetRendererBounds(this GameObject go) {
            return SceneUtility.GetBounds(go, true, false, false);
        }
    }
}
