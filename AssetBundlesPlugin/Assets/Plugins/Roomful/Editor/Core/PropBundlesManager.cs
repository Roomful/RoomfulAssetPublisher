using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using RF.AssetBundles.Serialization;
using RF.AssetBundles;

namespace RF.AssetWizzard.Editor {
	public static class PropBundleManager  {

		public static event System.Action AssetBundleDownloadedEvent = delegate{};
		public static event System.Action AssetBundleUploadedEvent = delegate{};

       

        private static void UploadAssetBundle(PropAsset prop) {

            EditorProgressBar.AddProgress("Requesting Thumbnail Upload Link", 0.1f);
            var getIconUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink_Thumbnail(prop.Template.Id);
            getIconUploadLink.PackageCallbackText = (linkCallback) => {

                EditorProgressBar.AddProgress("Uploading Asset Thumbnail", 0.1f);
                var uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset_Thumbnail(linkCallback, prop.Icon);

                float currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress("Uploading Asset Thumbnail", 0f);
                };

                uploadRequest.PackageCallbackText = (string uploadCallback) => {

                    EditorProgressBar.AddProgress("Waiting Thumbnail Upload Confirmation", 0.3f);
                    var confirmRequest = new Network.Request.UploadConfirmation_Thumbnail(prop.Template.Id);
                    confirmRequest.PackageCallbackText = (string resData) => {

                        var resInfo = new JSONData(resData);
                        var res = new Resource(resInfo);
                        prop.Template.Icon = res;

                        AssetBundlesSettings.Instance.ReplaceTemplate(prop.Template);
                        BundleUtility.GenerateUploadPrefab(prop);
                        AssetsUploadLoop(0, prop.Template);
                    };
                    confirmRequest.Send();
                };
                uploadRequest.Send();
            };
            getIconUploadLink.Send();
		}

        public static void AssetsUploadLoop(int platformIndex, PropTemplate tpl) {
			AssetBundlesSettings.Instance.UploadTemplate = tpl;
            AssetBundlesSettings.Instance.UploadPlatfromIndex = platformIndex;

            if (platformIndex < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
				BuildTarget pl = AssetBundlesSettings.Instance.TargetPlatforms [platformIndex];
                string prefabPath = AssetBundlesSettings.FULL_ASSETS_PREFABS_LOCATION + "temp/" + tpl.Title + ".prefab";
				string assetBundleName = tpl.Title + "_" + pl;

				assetBundleName = assetBundleName.ToLower ();

				AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
				assetImporter.assetBundleName = assetBundleName;

				FolderUtils.CreateFolder (AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
				BuildPipeline.BuildAssetBundles (AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, BuildAssetBundleOptions.UncompressedAssetBundle, pl);
			}
		}

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
			if(!AssetBundlesSettings.Instance.IsUploadInProgress) {
                return;
            }

			int platformIndex = AssetBundlesSettings.Instance.UploadPlatfromIndex;
            PropTemplate tpl = AssetBundlesSettings.Instance.UploadTemplate;
            BuildTarget platform = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];

            string assetBundleName = tpl.Title + "_" + platform;

            EditorProgressBar.AddProgress("Getting Asset Upload URL (" + platform + ")", 0.2f);
            var uploadLinkRequest = new RF.AssetWizzard.Network.Request.GetUploadLink(tpl.Id, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText = (linkCallback) => {

                EditorProgressBar.AddProgress("Uploading Asset (" + platform + ")", 0.2f);
                byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
                Network.Request.UploadAsset uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset(linkCallback, assetBytes);

                float currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress("Uploading Asset (" + platform + ")", 0f);
                };

                uploadRequest.PackageCallbackText = (uploadCallback) => {
                    EditorProgressBar.AddProgress("Waiting Asset Upload Confirmation (" + platform + ")", 0.2f / (float)AssetBundlesSettings.Instance.TargetPlatforms.Count);
                    Network.Request.UploadConfirmation confirm = new Network.Request.UploadConfirmation(tpl.Id, platform.ToString());
                    confirm.PackageCallbackText = (confirmCallback) => {
                        platformIndex++;
                        AssetDatabase.RemoveUnusedAssetBundleNames();

                        if (platformIndex == AssetBundlesSettings.Instance.TargetPlatforms.Count) {
							FinishAssetUpload();
                        } else {
							AssetsUploadLoop(platformIndex, tpl);
                        }
					};
                    confirm.Send();

                };
                uploadRequest.Send();

            };
            uploadLinkRequest.Send();
        }

		private static void FinishAssetUpload() {
	        PropTemplate tpl = AssetBundlesSettings.Instance.UploadTemplate;

            AssetBundlesSettings.Instance.UploadTemplate = new PropTemplate();
            AssetBundlesSettings.Save();

            BundleUtility.DelteTempFiles();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            EditorApplication.delayCall = () => {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION, false);
                FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            };

			if (AssetBundlesSettings.Instance.IsInAutoloading) {
                EditorProgressBar.FinishUploadProgress();
			} else { 
				PropBundleManager.DownloadAssetBundle(tpl, false);
                EditorProgressBar.FinishUploadProgress();
				EditorUtility.DisplayDialog ("Success", " Asset has been successfully uploaded!", "Ok");
			}

			AssetBundleUploadedEvent ();
        }

		public static void CreateNewAsset(PropTemplate tpl) {
			if (string.IsNullOrEmpty(tpl.Title)) {
				Debug.Log ("Prop's name is empty");
				return;
			}

			EditorApplication.delayCall = () => {
				EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
				WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

				PropAsset createdProp = new GameObject (tpl.Title).AddComponent<PropAsset> ();
				createdProp.SetTemplate(tpl);

				FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_PREFABS_LOCATION);
			};
		}

        private static AssetBundle CurrentAssetBundle = null;

        public static void DownloadAssetBundle(PropTemplate prop, bool saveSceneRequest = true) {

            if (AssetBundlesSettings.Instance.AutomaticCacheClean) {
                BundleUtility.ClearLocalCache();
            }

           
            EditorApplication.delayCall = () => {
				if(saveSceneRequest) {
					EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				}

                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

				string pl = EditorUserBuildSettings.activeBuildTarget.ToString();

				Network.Request.GetAssetUrl getAssetUrl = new RF.AssetWizzard.Network.Request.GetAssetUrl (prop.Id, pl);
                getAssetUrl.PackageCallbackText = (assetUrl) => {
                    					
                    Network.Request.DownloadAsset loadAsset = new RF.AssetWizzard.Network.Request.DownloadAsset (assetUrl);
                    loadAsset.PackageCallbackData = (loadCallback) => {
						
						if (!FolderUtils.IsFolderExists(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION)) {
		                    FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
						}
                        
						string bundlePath = AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION+"/"+prop.Title+"_"+pl;

						FolderUtils.WriteBytes(bundlePath, loadCallback);

                        Caching.ClearCache();
                        Resources.UnloadUnusedAssets();

						if (CurrentAssetBundle  != null) {
							CurrentAssetBundle.Unload(true);
                           
							CurrentAssetBundle = null;
						}
                        
                        CurrentAssetBundle = AssetBundle.LoadFromFile(bundlePath);

						RecreateProp(prop, CurrentAssetBundle.LoadAsset<Object>(prop.Title));

						AssetDatabase.DeleteAsset(bundlePath);

						AssetBundleDownloadedEvent();
					};

					loadAsset.Send ();
				};

				getAssetUrl.Send();
			};
		}

		public static void ReuploadAsset(PropAsset prop) {

			if(!Validation.Run(prop)) { return; }

            //just to mark that uploading process has started
            AssetBundlesSettings.Instance.UploadTemplate = prop.Template;

            prop.PrepareForUpload ();

            EditorProgressBar.StartUploadProgress("Updating Asset Template");
            RF.AssetWizzard.Network.Request.PropMetaDataUpdate updateRequest = new RF.AssetWizzard.Network.Request.PropMetaDataUpdate (prop.Template);
            updateRequest.PackageCallbackText = (updateCalback) => {
				UploadAssetBundle(prop);
			};
			updateRequest.Send ();
		}

		public static void UploadAssets(PropAsset prop) {

			if(!Validation.Run(prop)) { return; }

            //just to mark that uploading process has started
            AssetBundlesSettings.Instance.UploadTemplate = prop.Template;

            prop.PrepareForUpload ();

            EditorProgressBar.StartUploadProgress("Updating Asset Template");

            Network.Request.PropMetaDataCreate createMeta = new RF.AssetWizzard.Network.Request.PropMetaDataCreate (prop.Template);
            createMeta.PackageCallbackText = (callback) => { 
				prop.Template.Id =  new PropTemplate(callback).Id;
				UploadAssetBundle(prop);
			};

			createMeta.Send ();
		}

		private static void RecreateProp(PropTemplate tpl, Object prop) {
			if (prop == null) {
				Debug.Log ("Prop is null");

				return;
			}
				
			GameObject newGo = (GameObject)GameObject.Instantiate (prop) as GameObject;
			newGo.name = tpl.Title;

			PropAsset asset = newGo.AddComponent<PropAsset> ();
			asset.SetTemplate (tpl);

			PropDataBase.ClearOldDataFolder (asset);

			RunCollectors(asset);

			WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);
		}

		private static void RunCollectors(PropAsset asset) {
            new V1_RendererCollector().Run(asset); // Old renderer collector must be called ALWAYS earlier than Renderer collector!!!
            new RendererCollector().Run (asset);
		    new TextCollector().Run (asset);
			new MeshCollector().Run (asset);
		    new ComponentsCollector().Run (asset);
			new AnimationCollector().Run (asset);
            new AnimatorCollector().Run(asset);

            new V1_ThumbnailsCollector().Run (asset);
			new V1_MarkersCollector ().Run (asset);
		}
 
    }
}
