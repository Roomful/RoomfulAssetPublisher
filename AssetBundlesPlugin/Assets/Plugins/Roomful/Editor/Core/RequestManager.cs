using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor {

	public static class RequestManager  {

		public static void ReloadAssets(List<string> tags) {
			AssetBundlesSettings.Instance.LocalAssetTemplates.Clear ();

			RF.AssetWizzard.Network.Request.GetAllAssets allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (0, 5, tags);

			allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {

				List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

				foreach (object assetData in allAssetsList) {
					AssetTemplate at = new AssetTemplate(new JSONData(assetData).RawData);
					AssetBundlesSettings.Instance.LocalAssetTemplates.Add(at);
				}
				AssetBundlesSettings.Save();
			};

			allAssetsRequest.Send ();
		}

		public static void LoadMoreAssets(List<string> tags) {
			RF.AssetWizzard.Network.Request.GetAllAssets allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (AssetBundlesSettings.Instance.LocalAssetTemplates.Count, 5, tags);

			allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {

				List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

				foreach (object assetData in allAssetsList) {
					AssetTemplate at = new AssetTemplate(new JSONData(assetData).RawData);
					AssetBundlesSettings.Instance.LocalAssetTemplates.Add(at);
				}
				AssetBundlesSettings.Save();

				//EditorWindow window = EditorWindow.GetWindow<WizzardWindow>(true, "Asset Bundles Wizzard");


			};
			allAssetsRequest.Send ();
		}

		public static void RemoveAsset(AssetTemplate asset) {
			RF.AssetWizzard.Network.Request.RemoveAsset removeRequest = new RF.AssetWizzard.Network.Request.RemoveAsset (asset.Id);

			removeRequest.PackageCallbackData = (removeCallback) => {
				AssetBundlesSettings.Instance.RemoverFromLocalAssetTemplates(asset);
			};

			removeRequest.Send ();
		}



	}

}
