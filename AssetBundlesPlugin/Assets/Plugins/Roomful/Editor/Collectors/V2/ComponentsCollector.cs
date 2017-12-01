using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor
{
	public class ComponentsCollector : ICollector {

		public void Run(IAsset asset) {

            IRecreatableOnLoad[] scripts = asset.gameObject.GetComponentsInChildren<IRecreatableOnLoad>(true);
            foreach (var script in scripts) {
                CopySerializedComponent(script, script.gameObject);
                GameObject.DestroyImmediate(script as Component);
            }


            foreach (SerializedThumbnail thumbnail in asset.gameObject.GetComponentsInChildren<SerializedThumbnail>()) {
				thumbnail.gameObject.AddComponent<PropThumbnail> ();
			}

			foreach (SerializedMeshThumbnail meshThumbnail in asset.gameObject.GetComponentsInChildren<SerializedMeshThumbnail>()) {
				meshThumbnail.gameObject.AddComponent<PropMeshThumbnail> ();
			}


			foreach (SerializedFrame frame in asset.gameObject.GetComponentsInChildren<SerializedFrame>()) {
				frame.gameObject.AddComponent<PropFrame> ();
			}


            foreach (SerializedAnchor frame in asset.gameObject.GetComponentsInChildren<SerializedAnchor>()) {
                frame.gameObject.AddComponent<PropAnchor>();
            }

            foreach (SerializedStylePanel panel in asset.gameObject.GetComponentsInChildren<SerializedStylePanel>()) {
                panel.gameObject.AddComponent<StylePanel>();
            }

        }

        private void CopySerializedComponent(IRecreatableOnLoad original, GameObject destination) {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields) {
                field.SetValue(copy, field.GetValue(original));
            }
        }

    }
 
}

