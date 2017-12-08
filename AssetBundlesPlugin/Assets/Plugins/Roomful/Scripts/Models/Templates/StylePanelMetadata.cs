using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard
{
    [System.Serializable]
    public class StylePanelMetadata {

        public string Name = string.Empty;
        public Vector3 Size = Vector3.zero;
        public List<ColliderMetaData> CollidersMetaData = new List<ColliderMetaData>();
        
        public StylePanelMetadata(StylePanel panel) {
            Name = panel.name;
            Size = panel.Bounds.size;

            CreateCollidersMetaData(panel);
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

            List<object> collList = new List<object>();

            foreach (ColliderMetaData cmd in CollidersMetaData) {
                collList.Add(cmd.ToDictionary());
            }

            data.Add("colliders", collList);
            
            return data;
        }
        
        private void ParseTemplate(JSONData panelInfo) {
            Name = panelInfo.GetValue<string>("name");
            JSONData MobileGeometrySize = new JSONData(panelInfo.GetValue<Dictionary<string, object>>("size"));

            Size.x = MobileGeometrySize.GetValue<float>("x");
            Size.y = MobileGeometrySize.GetValue<float>("y");
            Size.z = MobileGeometrySize.GetValue<float>("z");

            if (panelInfo.HasValue("colliders")) {
                List<object> collList = panelInfo.GetValue<List<object>>("colliders");

                foreach (object coll in collList) {
                    JSONData parsedColl = new JSONData(coll);

                    CollidersMetaData.Add(new ColliderMetaData(parsedColl));
                }
            }
        }

        private void CreateCollidersMetaData(StylePanel panel) {
            BoxCollider[] colliders = panel.GetComponentsInChildren<BoxCollider>();

            foreach (BoxCollider bc in colliders) {
                ColliderMetaData newMetaData = new ColliderMetaData();

                Transform cachedParent = bc.transform.parent;
                bc.transform.SetParent(panel.transform);

                newMetaData.Position = bc.transform.localPosition;
                newMetaData.Rotation = bc.transform.localRotation.eulerAngles;
                newMetaData.Scale = bc.transform.localScale;
                newMetaData.Center = bc.center;
                newMetaData.Size = bc.size;
                newMetaData.MarkedAsDisable = bc.GetComponent<RF.AssetBundles.Serialization.SerializedDisabledAreaMarker>() != null;
                    
                CollidersMetaData.Add(newMetaData);

                bc.transform.SetParent(cachedParent);
            }
        }
    }
}