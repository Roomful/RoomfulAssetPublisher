using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


namespace RF.AssetWizzard.Editor {

	public class AssetBundlesManager : MonoBehaviour {



		public static void Clone(RF.AssetWizzard.PropAsset prop) {
			GameObject clone =  Instantiate(prop.gameObject);

			var p = clone.GetComponent<RF.AssetWizzard.PropAsset> ();
			DestroyImmediate (p);
			CreatePrefabClone (prop.Template.Title, clone);



			DestroyImmediate (clone);
			AssetDatabase.SaveAssets ();

		}


		public static void CreatePrefabClone(string name, GameObject source) {

			FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_LOCATION + "temp/");
			PrefabUtility.CreatePrefab (AssetBundlesSettings.FULL_ASSETS_LOCATION + "temp/" + name + ".prefab", source);
		}

		public static void CreatePrefab(string name, GameObject source) {
			PrefabUtility.CreatePrefab (AssetBundlesSettings.FULL_ASSETS_LOCATION + name + ".prefab", source);
		}


		public static void DelteTempFiles() {
			FolderUtils.DeleteFolder (AssetBundlesSettings.ASSETS_LOCATION +"temp/");
		}

		public static void DeletePrefab(string name) {
			FileUtil.DeleteFileOrDirectory(AssetBundlesSettings.FULL_ASSETS_LOCATION + name+".prefab");
		} 


		public static bool ValidateAsset(RF.AssetWizzard.PropAsset asset) {
			
			if (asset.Template.Thumbnail == null) {
				EditorUtility.DisplayDialog ("Error", "Set Asset thumbnail!", "Ok");
				return false;
			}

			if (asset.transform.childCount < 1) {
				EditorUtility.DisplayDialog ("Error", "Asset is empty!", "Ok");
				return false;
			}

			return true;

		}






		public static void AssetsUploadLoop(int i, AssetTemplate tpl, System.Action FinishHandler) {

			if (i < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
				BuildTarget pl = AssetBundlesSettings.Instance.TargetPlatforms [i];

				//BuildAssetBundleFor(tpl.Title, pl);

				string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + "temp/" + tpl.Title + ".prefab";
				string assetBundleName = tpl.Title + "_" + pl;
				assetBundleName = assetBundleName.ToLower ();


				AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
				assetImporter.assetBundleName = assetBundleName;

				FolderUtils.CreateFolder (AssetBundlesSettings.AssetBundlesPath);
				BuildPipeline.BuildAssetBundles (AssetBundlesSettings.AssetBundlesPathFull, BuildAssetBundleOptions.UncompressedAssetBundle, pl);
				AssetDatabase.Refresh ();

				Network.Request.GetUploadLink getUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink (tpl.Id, pl.ToString(), tpl.Title);

				getUploadLink.PackageCallbackText = (linkCallback) => {

					byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.AssetBundlesPathFull+ "/" + assetBundleName);

					Network.Request.UploadAsset uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset(linkCallback, assetBytes);

					uploadRequest.PackageCallbackText = (uploadCallback)=> {
						Network.Request.UploadConfirmation confirm = new Network.Request.UploadConfirmation(tpl.Id, pl.ToString());

						confirm.PackageCallbackText = (confirmCallback)=> {
							i++;

							CleanAssetBundleName(tpl.Title);

							if (i == AssetBundlesSettings.Instance.TargetPlatforms.Count) {
								FinishHandler();
							} else {
								AssetsUploadLoop(i, tpl, FinishHandler);
							}

						};

						confirm.Send();
					};

					uploadRequest.Send();
				};
				getUploadLink.Send();



			}
		}


		private static void CleanAssetBundleName(string assetName) {
			string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + assetName+ ".prefab";

			AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
			assetImporter.assetBundleName = string.Empty;

			AssetDatabase.RemoveUnusedAssetBundleNames ();
		}


		public static void CreateNewAsset(AssetTemplate tpl) {
			if (string.IsNullOrEmpty(tpl.Title)) {
				Debug.Log ("Prop's name is empty");
				return;
			}

			EditorApplication.delayCall = () => {

				EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
				WindowManager.Wizzard.SiwtchTab(WizzardTabs.Wizzard);

				string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + tpl.Title + ".prefab";
				PropAsset createdProp = new GameObject (tpl.Title).AddComponent<PropAsset> ();
				createdProp.SetTemplate(tpl);

				FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_LOCATION);
				GameObject newPrfab = PrefabUtility.CreatePrefab (prefabPath, createdProp.gameObject);
				PrefabUtility.ConnectGameObjectToPrefab (createdProp.gameObject, newPrfab);

			};
		}
			

	}

}
