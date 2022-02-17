using System.Collections.Generic;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api
{
    [System.Serializable]
    public class StyleAssetMetadata
    {
        public List<StyleAssetPanelMetadata> Panels = new List<StyleAssetPanelMetadata>();

        public StyleAssetMetadata() { }

        public StyleAssetMetadata(JSONData styleInfo) {
            ParseTemplate(styleInfo);
        }

        public Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object>();

            var panelsData = new List<Dictionary<string, object>>();
            foreach (var tpl in Panels) {
                panelsData.Add(tpl.ToDictionary());
            }

            data.Add("panels", panelsData);

            return data;
        }

        public StyleAssetPanelMetadata GetStartPanelMeta() {
            return Panels[0];
        }

        public StyleAssetPanelMetadata GetEndPanelMeta() {
            return Panels[Panels.Count - 1];
        }

        public StyleAssetPanelMetadata GetPanelMetaByName(string name) {
            foreach (var meta in Panels) {
                if (meta.Name.Equals(name)) {
                    return meta;
                }
            }

            return null;
        }

        private void ParseTemplate(JSONData styleInfo) {
            Panels = new List<StyleAssetPanelMetadata>();
            var panels = styleInfo.GetValue<List<object>>("panels");
            foreach (object panel in panels) {
                var panelInfo = new JSONData(panel);

                var tpl = new StyleAssetPanelMetadata(panelInfo);
                Panels.Add(tpl);
            }
        }
    }
}