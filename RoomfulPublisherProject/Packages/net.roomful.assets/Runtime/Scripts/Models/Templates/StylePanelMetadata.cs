using System.Collections.Generic;
using net.roomful.api;
using UnityEngine;

using net.roomful.assets.serialization;


namespace net.roomful.assets
{
    [System.Serializable]
    internal class StylePanelMetadata {

        public string Name = string.Empty;
        
        public StylePanelMetadata(StylePanel panel) {
            Name = panel.name;
        }

        public StylePanelMetadata(JSONData panelInfo) {
            ParseTemplate(panelInfo);
        }
        
        public Dictionary<string, object> ToDictionary() {

            var data = new Dictionary<string, object>();
            data.Add("name", Name);

            return data;
        }


        private List<object> GetCollidersDictionaryList(List<ColliderMetaData> meta) {
            var collList = new List<object>();
            foreach (var cmd in meta) {
                collList.Add(cmd.ToDictionary());
            }
            return collList;
        }

        private void ParseTemplate(JSONData panelInfo) {
            Name = panelInfo.GetValue<string>("name");
        }


        private List<ColliderMetaData> ParseColliders(JSONData data, string key) {
            var result = new List<ColliderMetaData>();
            if (data.HasValue(key)) {
                var collList = data.GetValue<List<object>>(key);

                foreach (var coll in collList) {
                    var parsedColl = new JSONData(coll);
                    result.Add(new ColliderMetaData(parsedColl));
                }
            }

            return result;
        }
        


        private List<ColliderMetaData> GetCollidersMeta(Transform tramsfrom) {

            var result = new List<ColliderMetaData>();
            var colliders = tramsfrom.GetComponentsInChildren<BoxCollider>();

            foreach (var bc in colliders) {
                var newMetaData = new ColliderMetaData();

                var cachedParent = bc.transform.parent;
                bc.transform.SetParent(tramsfrom);

                newMetaData.Position = bc.transform.localPosition;
                newMetaData.Rotation = bc.transform.localRotation.eulerAngles;
                newMetaData.Scale = bc.transform.localScale;
                newMetaData.Center = bc.center;
                newMetaData.Size = bc.size;
                newMetaData.MarkedAsDisable = bc.GetComponent<SerializedDisabledAreaMarker>() != null;
                newMetaData.Name = bc.name;

                result.Add(newMetaData);
                bc.transform.SetParent(cachedParent);

            }

            return result;
        }

    }
}