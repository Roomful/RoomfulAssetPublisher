using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public static class JSONDataUtility
    {
        public static Vector3 ParseVector3(string name, JSONData data) {
            var posData = new JSONData(data.GetValue<Dictionary<string, object>>(name));
            return new Vector3(
                posData.GetValue<float>("x"),
                posData.GetValue<float>("y"),
                posData.GetValue<float>("z")
            );
        }

        public static Dictionary<string, object> CreateVector3(Vector3 vector) {
            return new Dictionary<string, object> {
                { "x", vector.x },
                { "y", vector.y },
                { "z", vector.z }
            };
        }

        public static Vector2 ParseVector2(string name, JSONData data) {
            var posData = new JSONData(data.GetValue<Dictionary<string, object>>(name));
            return new Vector3(
                posData.GetValue<float>("x"),
                posData.GetValue<float>("y")
            );
        }

        public static Dictionary<string, object> CreateVector2(Vector2 vector) {
            return new Dictionary<string, object> {
                { "x", vector.x },
                { "y", vector.y }
            };
        }

        public static Color ParseRGBAColor(string name, JSONData data) {
            var posData = new JSONData(data.GetValue<Dictionary<string, object>>(name));
            return new Color(
                posData.GetValue<float>("r"),
                posData.GetValue<float>("g"),
                posData.GetValue<float>("b"),
                posData.GetValue<float>("a")
            );
        }

        public static Dictionary<string, object> CreateRGBAColor(Color color) {
            return new Dictionary<string, object> {
                { "r", color.r },
                { "g", color.g },
                { "b", color.b },
                { "a", color.a }
            };
        }
    }
}