using UnityEngine;
using net.roomful.assets.serialization;
using UnityEditor;

namespace net.roomful.assets.editor
{
    internal class ComponentsCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            RemoveMissingScripts(asset.gameObject);
            
            var scripts = asset.gameObject.GetComponentsInChildren<IRecreatableOnLoad>(true);
            foreach (var script in scripts) {
                CopySerializedComponent(script, script.gameObject);
                Object.DestroyImmediate(script as Component);
            }

            foreach (var thumbnail in asset.gameObject.GetComponentsInChildren<SerializedThumbnail>()) {
                thumbnail.gameObject.AddComponent<PropThumbnail>();
            }

            foreach (var meshThumbnail in asset.gameObject.GetComponentsInChildren<SerializedMeshThumbnail>()) {
                meshThumbnail.gameObject.AddComponent<PropMeshThumbnail>();
            }

            foreach (var frame in asset.gameObject.GetComponentsInChildren<SerializedFrame>()) {
                frame.gameObject.AddComponent<PropStretchedFrame>();
            }

            foreach (var frame in asset.gameObject.GetComponentsInChildren<SerializedTiledFrame>()) {
                frame.gameObject.AddComponent<PropTiledFrame>();
            }

            foreach (var frame in asset.gameObject.GetComponentsInChildren<SerializedAnchor>()) {
                frame.gameObject.AddComponent<PropAnchor>();
            }

            foreach (var panel in asset.gameObject.GetComponentsInChildren<SerializedStylePanel>()) {
                panel.gameObject.AddComponent<StylePanel>();
            }
        }

        private void CopySerializedComponent(IRecreatableOnLoad original, GameObject destination) {
           
            /*
            var type = original.GetType();
            var copy = destination.AddComponent(type);
            var fields = type.GetFields();
            foreach (var field in fields) {
                field.SetValue(copy, field.GetValue(original));
            }*/
            
            var type = original.GetType();
            var json = JsonUtility.ToJson(original);
            var copy = destination.AddComponent(type);
            JsonUtility.FromJsonOverwrite(json, copy);
        }

        private void RemoveMissingScripts(GameObject assetGameObject) {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(assetGameObject);
            foreach (Transform childT in assetGameObject.transform) {
                RemoveMissingScripts(childT.gameObject);
            }
        }
    }
}