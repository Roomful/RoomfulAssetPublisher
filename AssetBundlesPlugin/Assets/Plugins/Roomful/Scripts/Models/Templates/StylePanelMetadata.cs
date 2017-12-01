using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard
{
    [System.Serializable]
    public class StylePanelMetadata
    {

        public string Name = string.Empty;
        public Vector3 Size = Vector3.zero;

        public StylePanelMetadata(StylePanel panel) {
            Name = panel.name;
            Size = panel.Bounds.size;
        }

        public StylePanelMetadata(JSONData panelInfo) {
            ParseTemplate(panelInfo);
        }


        public Dictionary<string, object> ToDictionary() {

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("name", Name);

            Dictionary<string, object> size = new Dictionary<string, object>();
            size.Add("x", Size.x);
            size.Add("y", Size.y);
            size.Add("z", Size.z);

            data.Add("size", size);

            return data;
        }


        private void ParseTemplate(JSONData panelInfo) {

            Name = panelInfo.GetValue<string>("name");
            JSONData MobileGeometrySize = new JSONData(panelInfo.GetValue<Dictionary<string, object>>("size"));

            Size.x = MobileGeometrySize.GetValue<float>("x");
            Size.y = MobileGeometrySize.GetValue<float>("y");
            Size.z = MobileGeometrySize.GetValue<float>("z");
        }
    }
}