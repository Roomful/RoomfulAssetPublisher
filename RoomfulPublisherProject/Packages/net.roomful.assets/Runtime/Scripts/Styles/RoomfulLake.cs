#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [ExecuteInEditMode]
    public class RoomfulLake : LakePolygon, IRecreatableOnLoad
    {
        public override void Awake() {
            GeneratePolygon();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Roomful Assets/R.A.M/Lake")]
        public static void CreateRoomfulLake() {
            Selection.activeGameObject = CreatePolygon<RoomfulLake>(AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat")).gameObject;
        }
#endif
    }
}