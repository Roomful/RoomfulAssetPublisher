using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard
{

    [System.Serializable]
    public class StyleMetadata 
    {
        public List<StylePanelMetadata> Panels = new List<StylePanelMetadata>();


        public StyleMetadata(JSONData styleInfo) {
            ParseTemplate(styleInfo);
        }


        public StyleMetadata(StyleAsset asset) {

            Panels = new List<StylePanelMetadata>();
            foreach (var panel in asset.Panels) {
                Panels.Add(new StylePanelMetadata(panel));
            }
        }


        public Dictionary<string, object> ToDictionary() {

            Dictionary<string, object> data = new Dictionary<string, object>();


            List<Dictionary<string, object>> panlesData = new List<Dictionary<string, object>>();
            foreach (var tpl in Panels) {
                panlesData.Add(tpl.ToDictionary());
            }
            data.Add("panels", panlesData);


            return data;
        }


        private void ParseTemplate(JSONData styleInfo) {

            Panels = new List<StylePanelMetadata>();
            List<object> panels = styleInfo.GetValue<List<object>>("panels");
            foreach (object panel in panels) {
                JSONData panelInfo = new JSONData(panel);

                var tpl = new StylePanelMetadata(panelInfo);
                Panels.Add(tpl);
            }

        }

    }
}