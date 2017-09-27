using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Editor {
	public class AutomaticReloader {
		
		public static void ReloadAllAssets() {
			var allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets (0, 10, new List<string> (){"whisper"});
			AssetBundlesSettings.Instance.TemporaryAssetTemplates.Clear ();

			allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
				List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

				foreach (object assetData in allAssetsList) {
					AssetTemplate at = new AssetTemplate(new JSONData(assetData).RawData);
					AssetBundlesSettings.Instance.TemporaryAssetTemplates.Add(at);
				}

				AssetBundlesSettings.Save();

				StartLoop();
			};

			allAssetsRequest.Send();
		}

		private static void StartLoop() {
			if (AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count > 0) {
				AssetBundlesSettings.Instance.IsInAutoloading = true;
				AssetBundlesManager.AssetBundleDownloadedEvent += AssetBundleDownloadedHandler;
				AssetBundlesManager.DownloadAssetBundle (Dequeue ());
			} else {
				AssetBundlesSettings.Instance.IsInAutoloading = false;
			}
		}

		private static void AssetBundleDownloadedHandler() {
			AssetBundlesManager.AssetBundleDownloadedEvent -= AssetBundleDownloadedHandler;

			UnityEditor.EditorApplication.update += OnUpdate;
		}

		private static int counter = 0;
		private static void OnUpdate() {
			counter += 1;

			if (counter > 360) {
				#if UNITY_EDITOR
				counter = 0;
				UnityEditor.EditorApplication.update -= OnUpdate;
				if (AssetBundlesSettings.Instance.IsInAutoloading) {
					AssetBundlesManager.ReuploadAsset (CurrentProp);
				}
				#endif
			}
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded() {
			#if UNITY_EDITOR
			if (AssetBundlesSettings.Instance.IsInAutoloading) {
				AssetBundlesManager.AssetBundleUploadedEvent += AssetBundleUploadedHandler;
			}
			#endif
		}

		private static void AssetBundleUploadedHandler() {
			AssetBundlesManager.AssetBundleUploadedEvent -= AssetBundleUploadedHandler;

			StartLoop ();
		}

		private static PropAsset CurrentProp {
			get {
				return GameObject.FindObjectOfType<PropAsset> ();
			}
		}

		private static AssetTemplate Dequeue() {
			int indexOfLast = AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count - 1;
			AssetTemplate curr = AssetBundlesSettings.Instance.TemporaryAssetTemplates [indexOfLast];

			AssetBundlesSettings.Instance.TemporaryAssetTemplates.Remove (curr);
			AssetBundlesSettings.Save();

			return curr;
		}


		private static AssetTemplate Peek() {
			int indexOfLast = AssetBundlesSettings.Instance.TemporaryAssetTemplates.Count - 1;

			return AssetBundlesSettings.Instance.TemporaryAssetTemplates [indexOfLast];
		}
	}
}
