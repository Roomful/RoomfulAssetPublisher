using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor
{
	public class EnvironmentCollector : ICollector {

		public void Run(IAsset asset) {
			
			foreach (SerializedEnviromnent e in asset.gameObject.GetComponentsInChildren<SerializedEnviromnent>(true)) {
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