using System.Collections.Generic;

// Copyright Roomful 2013-2018. All rights reserved.


namespace net.roomful.api
{
    [System.Serializable]
    public class StyleAssetPanelMetadata
    {

        public string Name = string.Empty;
        public List<ColliderMetaData> WallCollidersMetaData = new List<ColliderMetaData>();
        public List<ColliderMetaData> CeilingCollidersMetaData = new List<ColliderMetaData>();
        public List<ColliderMetaData> FloorCollidersMetaData = new List<ColliderMetaData>();

        public StyleAssetPanelMetadata(JSONData panelInfo) {
            ParseTemplate(panelInfo);
        }


        public Dictionary<string, object> ToDictionary() {

            var data = new Dictionary<string, object>();
            data.Add("name", Name);
            data.Add("wall_colliders", GetCollidersDictionaryList(WallCollidersMetaData));
            data.Add("floor_colliders", GetCollidersDictionaryList(FloorCollidersMetaData));
            data.Add("ceiling_colliders", GetCollidersDictionaryList(CeilingCollidersMetaData));

            return data;
        }

        private List<object> GetCollidersDictionaryList(List<ColliderMetaData> meta) {
            List<object> collList = new List<object>();
            foreach (ColliderMetaData cmd in meta) {
                collList.Add(cmd.ToDictionary());
            }
            return collList;
        }


        private void ParseTemplate(JSONData panelInfo) {
            Name = panelInfo.GetValue<string>("name");
            
            // Parsing is disabled since we no longer using silhouettes.
            /*
            WallCollidersMetaData = ParseColliders(panelInfo, "wall_colliders");
            FloorCollidersMetaData = ParseColliders(panelInfo, "floor_colliders");
            CeilingCollidersMetaData = ParseColliders(panelInfo, "ceiling_colliders");
            */
        }

        private List<ColliderMetaData> ParseColliders(JSONData data, string key) {
            List<ColliderMetaData> result = new List<ColliderMetaData>();
            if (data.HasValue(key)) {
                List<object> collList = data.GetValue<List<object>>(key);

                foreach (object coll in collList) {
                    JSONData parsedColl = new JSONData(coll);
                    result.Add(new ColliderMetaData(parsedColl));
                }
            }

            return result;
        }

    }
}