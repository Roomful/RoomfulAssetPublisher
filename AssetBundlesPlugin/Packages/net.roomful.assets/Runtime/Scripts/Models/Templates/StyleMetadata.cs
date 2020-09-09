using System.Collections.Generic;
using net.roomful.api;

namespace net.roomful.assets
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

            var data = new Dictionary<string, object>();


            var panlesData = new List<Dictionary<string, object>>();
            foreach (var tpl in Panels) {
                panlesData.Add(tpl.ToDictionary());
            }
            data.Add("panels", panlesData);


            return data;
        }


        private void ParseTemplate(JSONData styleInfo) {

            Panels = new List<StylePanelMetadata>();
            var panels = styleInfo.GetValue<List<object>>("panels");
            foreach (var panel in panels) {
                var panelInfo = new JSONData(panel);

                var tpl = new StylePanelMetadata(panelInfo);
                Panels.Add(tpl);
            }

        }

    }
}