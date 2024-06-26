using net.roomful.assets;
using net.roomful.assets.serialization;
using UnityEngine;
using UnityEditor;

namespace RF.AssetBundles.Serialization
{
    [CustomEditor(typeof(SerializedAnimationMarker), true)]
    public class AnimationMarkerInspector : Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            var click = GUILayout.Button("Add user click area", EditorStyles.miniButton, GUILayout.Width(200));
            if (click) {
                var parentGo = new GameObject("UserClickMarker");
                parentGo.transform.SetParent(Model.transform);
                parentGo.transform.localPosition = Vector3.zero;
                parentGo.AddComponent<UserClickMarker>();
                Model.TargetClickArea = parentGo.transform;
                var go = PrefabManager.CreatePrefab("Room/PropAnimationButton");
                go.transform.SetParent(parentGo.transform);
                go.transform.localPosition = Vector3.zero;
            }
        }
        
        private SerializedAnimationMarker Model => (SerializedAnimationMarker) target;
    }
}