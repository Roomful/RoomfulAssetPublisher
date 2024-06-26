#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [ExecuteInEditMode]
    public class RoomfulRiver : RamSpline, IRecreatableOnLoad
    {
        public override void Start() {
            base.Start();

            GenerateSpline();
        }
        
#if UNITY_EDITOR
        [MenuItem("GameObject/Roomful Assets/R.A.M/River")]
        public static void CreateRoomfulLake() {
            Selection.activeGameObject = CreateSpline<RoomfulRiver>(AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat")).gameObject;
        }
#endif
    }
}