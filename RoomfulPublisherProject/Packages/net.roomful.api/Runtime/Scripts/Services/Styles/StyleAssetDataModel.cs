using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public class StyleAssetDataModel
    {
        public Vector3 HomePosition { get; set; } = Vector3.zero;
        public StyleType StyleType { get; set; } = StyleType.Default;
        public StyleDoorsType DoorsType { get; set; } = StyleDoorsType.Glass;
        public float Price { get; set; } = 1.23f;

        public StyleAssetDataModel() { }

        public StyleAssetDataModel(JSONData assetData) {
            if (assetData.HasValue("price")) {
                Price = assetData.GetValue<float>("price");
            }

            if (assetData.HasValue("styleType")) {
                var styleTypeString = assetData.GetValue<string>("styleType");
                if (!string.IsNullOrEmpty(styleTypeString)) {
                    StyleType = EnumUtility.ParseEnum<StyleType>(styleTypeString);
                }
            }

            if (assetData.HasValue("doorsType")) {
                var doorsTypeString = assetData.GetValue<string>("doorsType");
                if (!string.IsNullOrEmpty(doorsTypeString)) {
                    DoorsType = EnumUtility.ParseEnum<StyleDoorsType>(doorsTypeString);
                }
            }

            if (assetData.HasValue("homePosition")) {
                var homePositionData = new JSONData(assetData.GetValue<Dictionary<string, object>>("homePosition"));
                HomePosition = new Vector3 {
                    x = homePositionData.GetValue<float>("x"),
                    y = homePositionData.GetValue<float>("y"),
                    z = homePositionData.GetValue<float>("z")
                };
            }
        }

        public Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object>();

            AppendDictionary(data);
            return data;
        }

        public void AppendDictionary(Dictionary<string, object> data) {
            data.Add("styleType", StyleType.ToString());
            data.Add("doorsType", DoorsType.ToString());

            var homePositionData = new Dictionary<string, object>();
            homePositionData.Add("x", HomePosition.x);
            homePositionData.Add("y", HomePosition.y);
            homePositionData.Add("z", HomePosition.z);
            data.Add("homePosition", homePositionData);
        }
    }
}
