using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {

    [System.Serializable]
    public class ColliderMetaData {

        public string Name = string.Empty;

        public Vector3 Position = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;
        public Vector3 Scale = Vector3.one;

        public Vector3 Center = Vector3.zero;
        public Vector3 Size = Vector3.one;

        public bool MarkedAsDisable = false;

        public ColliderMetaData() {
            
        }

        public ColliderMetaData(JSONData data) {
            Parse(data);
        }

        public Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> data = new Dictionary<string, object>();

            Dictionary<string, object> pos = new Dictionary<string, object>();
            pos.Add("x", Position.x);
            pos.Add("y", Position.y);
            pos.Add("z", Position.z);

            Dictionary<string, object> rot = new Dictionary<string, object>();
            rot.Add("x", Rotation.x);
            rot.Add("y", Rotation.y);
            rot.Add("z", Rotation.z);

            Dictionary<string, object> sca = new Dictionary<string, object>();
            sca.Add("x", Scale.x);
            sca.Add("y", Scale.y);
            sca.Add("z", Scale.z);

            Dictionary<string, object> cen = new Dictionary<string, object>();
            cen.Add("x", Center.x);
            cen.Add("y", Center.y);
            cen.Add("z", Center.z);

            Dictionary<string, object> size = new Dictionary<string, object>();
            size.Add("x", Size.x);
            size.Add("y", Size.y);
            size.Add("z", Size.z);

            data.Add("position", pos);
            data.Add("rotation", rot);
            data.Add("scale", sca);
            data.Add("center", cen);
            data.Add("size", size);

            data.Add("disabled", MarkedAsDisable);
            data.Add("name", Name);

            return data;
        }

        private void Parse(JSONData data) {
            JSONData posData = new JSONData(data.GetValue<Dictionary<string, object>>("position"));
            Position.x = posData.GetValue<float>("x");
            Position.y = posData.GetValue<float>("y");
            Position.z = posData.GetValue<float>("z");

            JSONData rotData = new JSONData(data.GetValue<Dictionary<string, object>>("rotation"));
            Rotation.x = rotData.GetValue<float>("x");
            Rotation.y = rotData.GetValue<float>("y");
            Rotation.z = rotData.GetValue<float>("z");

            JSONData scaData = new JSONData(data.GetValue<Dictionary<string, object>>("scale"));
            Scale.x = scaData.GetValue<float>("x");
            Scale.y = scaData.GetValue<float>("y");
            Scale.z = scaData.GetValue<float>("z");

            JSONData cenData = new JSONData(data.GetValue<Dictionary<string, object>>("center"));
            Center.x = cenData.GetValue<float>("x");
            Center.y = cenData.GetValue<float>("y");
            Center.z = cenData.GetValue<float>("z");

            JSONData sizData = new JSONData(data.GetValue<Dictionary<string, object>>("size"));
            Size.x = sizData.GetValue<float>("x");
            Size.y = sizData.GetValue<float>("y");
            Size.z = sizData.GetValue<float>("z");

            MarkedAsDisable = data.GetValue<bool>("disabled");
            Name = data.GetValue<string>("name");
        }
    }
}