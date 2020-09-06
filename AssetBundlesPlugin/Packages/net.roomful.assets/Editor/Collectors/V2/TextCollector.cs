using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets.Editor
{
	public class TextCollector : BaseCollector {

		public override void Run(IAsset asset) {
			
			foreach (SerializedText textInfo in asset.gameObject.GetComponentsInChildren<SerializedText>(true)) {
				if(textInfo.FontFileContent  != null && textInfo.FontFileContent.Length > 0) {
					AssetDatabase.SaveFontAsset(asset, textInfo); 
					textInfo.Font = AssetDatabase.LoadFontAsset (asset, textInfo);
				} 
				var text =  textInfo.gameObject.AddComponent<RoomfulText>();
				text.Restore(textInfo);
				GameObject.DestroyImmediate(textInfo);
			}
		}
	}
}