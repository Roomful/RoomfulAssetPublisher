using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor
{
	public class TextCollector : ICollector {

		public void Run(IAsset asset) {
			
			foreach (SerializedText textInfo in asset.gameObject.GetComponentsInChildren<SerializedText>(true)) {
				if(textInfo.FontFileContent  != null && textInfo.FontFileContent.Length > 0) {
					AssetDatabase.SaveFontAsset(asset, textInfo); 

					textInfo.Font = AssetDatabase.LoadFontAsset (asset, textInfo);
				
				} else {
					//Debug.Log("no font content");
				}

				var text =  textInfo.gameObject.AddComponent<RoomfulText>();
				text.Restore(textInfo);
				GameObject.DestroyImmediate(textInfo);
			}
		}
	}
}