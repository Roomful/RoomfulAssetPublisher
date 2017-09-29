using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{
	public class TextCollector : ICollector {

		public void Run(PropAsset propAsset) {
			
			foreach (SerializedText textInfo in propAsset.gameObject.GetComponentsInChildren<SerializedText>()) {
				if(textInfo.FontFileContent  != null && textInfo.FontFileContent.Length > 0) {
					PropDataBase.SaveFontAsset(propAsset, textInfo); 

					textInfo.Font = PropDataBase.LoadFontAsset (propAsset, textInfo);
				
				} else {
					Debug.Log("no font content");
				}

				var text =  textInfo.gameObject.AddComponent<RoomfulText>();
				text.Restore(textInfo);
				GameObject.DestroyImmediate(textInfo);
			}
		}
	}
}