using System.Collections.Generic;

namespace net.roomful.assets
{
    static class AssetHierarchySettings
    {
        static List<string> s_HierarchyLayers;

        public static List<string> HierarchyLayers {
            get {
                if (s_HierarchyLayers == null) {
                    s_HierarchyLayers = new List<string>();
                    foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
                        s_HierarchyLayers.Add(layer.ToString());
                    }
                }

                return s_HierarchyLayers;
            }
        }
    }
}