using System.Collections.Generic;
using net.roomful.api;
using UnityEngine;

using net.roomful.assets.serialization;


namespace net.roomful.assets
{
    [System.Serializable]
    public class StylePanelMetadata {

        public string Name = string.Empty;
        public Vector3 Size = Vector3.zero;

        public List<ColliderMetaData> WallCollidersMetaData = new List<ColliderMetaData>();
        public List<ColliderMetaData> CeilingCollidersMetaData = new List<ColliderMetaData>();
        public List<ColliderMetaData> FloorCollidersMetaData = new List<ColliderMetaData>();
        
        public StylePanelMetadata(StylePanel panel) {
            Name = panel.name;
            Size = panel.Bounds.size;

            CreateCollidersMetaData(panel);
        }

        public StylePanelMetadata(JSONData panelInfo) {
            ParseTemplate(panelInfo);
        }
        
        public Dictionary<string, object> ToDictionary() {

            var data = new Dictionary<string, object>();
            data.Add("name", Name);

            var size = new Dictionary<string, object>();
            size.Add("x", Size.x);
            size.Add("y", Size.y);
            size.Add("z", Size.z);

            data.Add("size", size);

    
            data.Add("wall_colliders", GetCollidersDictionaryList(WallCollidersMetaData));
            data.Add("floor_colliders", GetCollidersDictionaryList(FloorCollidersMetaData));
            data.Add("ceiling_colliders", GetCollidersDictionaryList(CeilingCollidersMetaData));



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
            var MobileGeometrySize = new JSONData(panelInfo.GetValue<Dictionary<string, object>>("size"));

            Size.x = MobileGeometrySize.GetValue<float>("x");
            Size.y = MobileGeometrySize.GetValue<float>("y");
            Size.z = MobileGeometrySize.GetValue<float>("z");


            WallCollidersMetaData = ParseColliders(panelInfo, "wall_colliders");
            FloorCollidersMetaData = ParseColliders(panelInfo, "floor_colliders");
            CeilingCollidersMetaData = ParseColliders(panelInfo, "ceiling_colliders");
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

        private void CreateCollidersMetaData(StylePanel panel) {
            WallCollidersMetaData = GetCollidersMeta(panel.Wall);
            FloorCollidersMetaData = GetCollidersMeta(panel.Floor);
            CeilingCollidersMetaData = GetCollidersMeta(panel.Ceiling);
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