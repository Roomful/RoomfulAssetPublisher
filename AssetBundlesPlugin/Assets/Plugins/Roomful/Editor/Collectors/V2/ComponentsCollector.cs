using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	public class ComponentsCollector : ICollector {

		public void Run(PropAsset propAsset) {

            IRecreatableOnLoad[] scripts = propAsset.gameObject.GetComponentsInChildren<IRecreatableOnLoad>();
            foreach (var script in scripts) {
                CopySerializedComponent(script, script.gameObject);
                GameObject.DestroyImmediate(script as Component);
            }


            foreach (SerializedThumbnail thumbnail in propAsset.gameObject.GetComponentsInChildren<SerializedThumbnail>()) {
				thumbnail.gameObject.AddComponent<PropThumbnail> ();
			}

			foreach (SerializedMeshThumbnail meshThumbnail in propAsset.gameObject.GetComponentsInChildren<SerializedMeshThumbnail>()) {
				meshThumbnail.gameObject.AddComponent<PropMeshThumbnail> ();
			}


			foreach (SerializedFrame frame in propAsset.gameObject.GetComponentsInChildren<SerializedFrame>()) {
				frame.gameObject.AddComponent<PropFrame> ();
			}


            foreach (SerializedAnchor frame in propAsset.gameObject.GetComponentsInChildren<SerializedAnchor>()) {
                frame.gameObject.AddComponent<PropAnchor>();
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

