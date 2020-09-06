﻿using System.Collections.Generic;


namespace net.roomful.assets
{
    public static class AssetHierarchySettings
    {
        private static List<string> _HierarchyLayers = null;

        public static List<string> HierarchyLayers
        {
            get
            {
                if (_HierarchyLayers == null)
                {
                    _HierarchyLayers = new List<string>();
                    foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers)))
                    {
                        _HierarchyLayers.Add(layer.ToString());
                    }
                }

                return _HierarchyLayers;
            }
        }
    }
}