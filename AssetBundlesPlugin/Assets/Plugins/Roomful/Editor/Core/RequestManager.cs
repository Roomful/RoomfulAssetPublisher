using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor {

	public static class RequestManager  {



        public static bool ASSETS_SEARTCH_IN_PROGRESS = false;
		public static void SeartchAssets() {

            ASSETS_SEARTCH_IN_PROGRESS = true;

            RF.AssetWizzard.Network.Request.GetAllAssets allAssetsRequest = null;

			if(AssetBundlesSettings.Instance.SeartchType == SeartchRequestType.ByName) {
				allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (AssetBundlesSettings.Instance.LocalAssetTemplates.Count, 5, AssetBundlesSettings.Instance.SeartchPattern);
			}


			if(AssetBundlesSettings.Instance.SeartchType == SeartchRequestType.ById) {
				allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (AssetBundlesSettings.Instance.LocalAssetTemplates.Count, 5, string.Empty);
				allAssetsRequest.SetId (AssetBundlesSettings.Instance.SeartchPattern);
			}

			if(AssetBundlesSettings.Instance.SeartchType == SeartchRequestType.ByTag) {
				List<string> separatedTags = new List<string>(AssetBundlesSettings.Instance.SeartchPattern.Split(','));
				allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (AssetBundlesSettings.Instance.LocalAssetTemplates.Count, 5, separatedTags);
			}


			allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
				List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

				foreach (object assetData in allAssetsList) {
					AssetTemplate at = new AssetTemplate(new JSONData(assetData).RawData);
					AssetBundlesSettings.Instance.LocalAssetTemplates.Add(at);
				}
				AssetBundlesSettings.Save();

                ASSETS_SEARTCH_IN_PROGRESS = false;
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
