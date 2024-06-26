using RF.AssetBundles;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// Unity GameObject Extensions
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns Game object renderer bounds including it's children.
        /// </summary>
        /// <param name="go">Target game object.</param>
        /// <returns></returns>
        public static Bounds GetRendererBounds(this GameObject go) {
            return SceneUtility.GetBounds(go, true, false, false);
        }
    }
}
