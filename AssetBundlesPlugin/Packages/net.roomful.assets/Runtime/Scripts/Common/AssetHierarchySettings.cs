using System.Collections.Generic;

namespace net.roomful.assets
{
    public static class AssetHierarchySettings
    {
        private static List<string> s_hierarchyLayers = null;

        public static List<string> HierarchyLayers {
            get {
                if (s_hierarchyLayers == null) {
                    s_hierarchyLayers = new List<string>();
                    foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
                        s_hierarchyLayers.Add(layer.ToString());
                    }
                }

                return s_hierarchyLayers;
            }
        }
    }
}