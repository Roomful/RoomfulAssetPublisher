using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets.Editor
{
	public class EnvironmentCollector : BaseCollector {

		public override void Run(IAsset asset) {
			
			foreach (SerializedEnvironment e in asset.gameObject.GetComponentsInChildren<SerializedEnvironment>(true)) {
				if(e.ReflectionCubemapFileData  != null && e.ReflectionCubemapFileData.Length > 0) {
                    e.ReflectionCubemap = null;
                    string path = AssetDatabase.SaveCubemapAsset(asset, e);
                    TextureCollector.ApplyImportSettings(path, e.ReflectionCubemapSettings);
                    
					e.ReflectionCubemap = AssetDatabase.LoadCubemapAsset(asset, e);
                } 
			}
		}
	}
}