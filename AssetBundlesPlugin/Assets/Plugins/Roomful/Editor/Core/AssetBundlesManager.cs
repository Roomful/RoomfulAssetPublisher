using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using RF.AssetBundles.Serialisation;


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
			

			float max = Mathf.Max (asset.Size.x, asset.Size.y, asset.Size.z);
			if(max < AssetBundlesSettings.MIN_ALLOWED_SIZE) {
				EditorUtility.DisplayDialog ("Error", "Your Asset is too small", "Ok");
				return false;
			}

			if(max > AssetBundlesSettings.MAX_AlLOWED_SIZE) {
				EditorUtility.DisplayDialog ("Error", "Your Asset is too big", "Ok");
				return false;
			}
	
			if (asset.Model.childCount < 1) {
				EditorUtility.DisplayDialog ("Error", "Asset is empty!", "Ok");
				return false;
			}

			return true;
		}


        private static float s_uploadProgress = 0f;

        public static void StartUploadProgress(string message) {
            s_uploadProgress = 0f;
            EditorUtility.DisplayProgressBar("Asset Upload", message, s_uploadProgress);
        }

        private static void AddProgress(string message, float progress) {
          
            s_uploadProgress += progress;
           // Debug.Log("AddProgress UI: " + s_uploadProgress + " / " + progress);
            EditorUtility.DisplayProgressBar("Asset Upload", message, s_uploadProgress);
        }

        private static void FinishUploadProgress() {
            EditorUtility.ClearProgressBar();
        }

        private static void UploadAssetBundle(PropAsset prop) {

            AddProgress("Requesting Thumbnail Upload Link", 0.1f);
            var getIconUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink_Thumbnail(prop.Template.Id);
            getIconUploadLink.PackageCallbackText = (linkCallback) => {

                AddProgress("Uploading Asset Thumbnail", 0.1f);
                var uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset_Thumbnail(linkCallback, prop.Icon);

                float currentUploadProgress = s_uploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    s_uploadProgress = currentUploadProgress + p;
                    AddProgress("Uploading Asset Thumbnail", 0f);
                };

                uploadRequest.PackageCallbackText = (string uploadCallback) => {

                    AddProgress("Waiting Thumbnail Upload Confirmation", 0.3f);
                    var confirmRequest = new Network.Request.UploadConfirmation_Thumbnail(prop.Template.Id);
                    confirmRequest.PackageCallbackText = (string resData) => {


                        var resInfo = new JSONData(resData);
                        var res = new Resource(resInfo);

                        prop.Template.Icon = res;
                        AssetBundlesSettings.Instance.ReplaceTemplate(prop.Template);

                        AssetBundlesManager.Clone(prop);
                        AssetBundlesManager.AssetsUploadLoop(0, prop.Template);
                    };
                    confirmRequest.Send();
                };
                uploadRequest.Send();
            };
            getIconUploadLink.Send();


        }

        public static void AssetsUploadLoop(int platformIndex, AssetTemplate tpl) {

            AssetBundlesSettings.Instance.UploadTemplate = tpl;
            AssetBundlesSettings.Instance.UploadPlatfromIndex = platformIndex;


            if (platformIndex < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
				BuildTarget pl = AssetBundlesSettings.Instance.TargetPlatforms [platformIndex];
                string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + "temp/" + tpl.Title + ".prefab";
				string assetBundleName = tpl.Title + "_" + pl;
				assetBundleName = assetBundleName.ToLower ();


				AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
				assetImporter.assetBundleName = assetBundleName;

				FolderUtils.CreateFolder (AssetBundlesSettings.AssetBundlesPath);
				BuildPipeline.BuildAssetBundles (AssetBundlesSettings.AssetBundlesPathFull, BuildAssetBundleOptions.UncompressedAssetBundle, pl);
                 //AssetDatabase.Refresh ();

			}
		}


        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {


            if(!AssetBundlesSettings.Instance.IsUploadInProgress) {
                return;
            }
          

            int platformIndex = AssetBundlesSettings.Instance.UploadPlatfromIndex;
            AssetTemplate tpl = AssetBundlesSettings.Instance.UploadTemplate;
            BuildTarget platform = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];
            string assetBundleName = tpl.Title + "_" + platform;


            AddProgress("Getting Asset Upload URL (" + platform + ")", 0.2f);
            var uploadLinkRequest = new RF.AssetWizzard.Network.Request.GetUploadLink(tpl.Id, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText = (linkCallback) => {

                AddProgress("Uploading Asset (" + platform + ")", 0.2f);
                byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.AssetBundlesPathFull + "/" + assetBundleName);
                Network.Request.UploadAsset uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset(linkCallback, assetBytes);


                float currentUploadProgress = s_uploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    s_uploadProgress = currentUploadProgress + p;
                    AddProgress("Uploading Asset (" + platform + ")", 0f);
                };

                uploadRequest.PackageCallbackText = (uploadCallback) => {

                    AddProgress("Waiting Asset Upload Confirmation (" + platform + ")", 0.2f / (float)AssetBundlesSettings.Instance.TargetPlatforms.Count);
                    Network.Request.UploadConfirmation confirm = new Network.Request.UploadConfirmation(tpl.Id, platform.ToString());
                    confirm.PackageCallbackText = (confirmCallback) => {
                        platformIndex++;
                        CleanAssetBundleName(tpl.Title);

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
            AssetTemplate tpl = AssetBundlesSettings.Instance.UploadTemplate;
            AssetBundlesSettings.Instance.UploadTemplate = new AssetTemplate();
            AssetBundlesSettings.Save();

            AssetBundlesManager.DelteTempFiles();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            EditorApplication.delayCall = () => {
                FolderUtils.DeleteFolder(AssetBundlesSettings.AssetBundlesPath, false);
                FolderUtils.CreateFolder(AssetBundlesSettings.AssetBundlesPath);
            };

            AssetBundlesManager.LoadAssetBundle(tpl, false);
            FinishUploadProgress();
            EditorUtility.DisplayDialog("Success", " Asset has been successfully uploaded!", "Ok");
        }


        private static void CleanAssetBundleName(string assetName) {
			AssetDatabase.RemoveUnusedAssetBundleNames ();
		}


		public static void CreateNewAsset(AssetTemplate tpl) {
			if (string.IsNullOrEmpty(tpl.Title)) {
				Debug.Log ("Prop's name is empty");
				return;
			}

			EditorApplication.delayCall = () => {




				EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
				WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

				string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + tpl.Title + ".prefab";
				PropAsset createdProp = new GameObject (tpl.Title).AddComponent<PropAsset> ();
				createdProp.SetTemplate(tpl);

				FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_LOCATION);
				GameObject newPrfab = PrefabUtility.CreatePrefab (prefabPath, createdProp.gameObject);
				PrefabUtility.ConnectGameObjectToPrefab (createdProp.gameObject, newPrfab);

			};
		}


		private static AssetBundle CurrentAssetBundle = null;

		public static void LoadAssetBundle(AssetTemplate prop, bool saveSceneRequest = true) {
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
						
						if (!FolderUtils.IsFolderExists(AssetBundlesSettings.AssetBundlesPathFull)) {
		                    FolderUtils.CreateFolder(AssetBundlesSettings.AssetBundlesPath);
						}
                        string bundlePath = AssetBundlesSettings.AssetBundlesPathFull+"/"+prop.Title+"_"+pl;

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
					};

					loadAsset.Send ();
				};

				getAssetUrl.Send();
			};
		}

		public static void ReUploadAsset(PropAsset prop) {

			if(!AssetBundlesManager.ValidateAsset(prop)) { return; 	}

            //just to mark that uploading process has started
            AssetBundlesSettings.Instance.UploadTemplate = prop.Template;

            prop.PrepareForUpload ();

            StartUploadProgress("Updating Asset Template");
            RF.AssetWizzard.Network.Request.UpdateAsset updateRequest = new RF.AssetWizzard.Network.Request.UpdateAsset (prop.Template);
            updateRequest.PackageCallbackText = (updateCalback) => {
				UploadAssetBundle(prop);
			};
			updateRequest.Send ();
		}

		public static void UploadAssets(PropAsset prop) {

			if(!AssetBundlesManager.ValidateAsset(prop)) { return; }

            //just to mark that uploading process has started
            AssetBundlesSettings.Instance.UploadTemplate = prop.Template;

            prop.PrepareForUpload ();

            StartUploadProgress("Updating Asset Template");
            Network.Request.CreateMetaData createMeta = new RF.AssetWizzard.Network.Request.CreateMetaData (prop.Template);
            createMeta.PackageCallbackText = (callback) => { 
				prop.Template.Id =  new AssetTemplate(callback).Id;
				UploadAssetBundle(prop);
			};

			createMeta.Send ();
		}
			
		public static void CheckAnimations(PropAsset prop) {
			Animator[] anims = prop.GetComponentsInChildren<Animator> ();
			Animator mainAnimator = null;

			int AnimatorsNumber = anims.Length;

			if (AnimatorsNumber > 1) {
				Debug.Log ("Animators number is more than 1");
				mainAnimator = anims [0];
			} else if (AnimatorsNumber == 1) {
				mainAnimator = anims [0];
			}

			if (mainAnimator != null) {
				Debug.Log ("Parameters:");
				for (int i = 0; i < mainAnimator.parameterCount; i++) {
					string n = mainAnimator.GetParameter (i).name;
					string t = mainAnimator.GetParameter (i).type.ToString();

					string log = "Parameter: " + n + ", type: " + t +", default: ";

					switch(mainAnimator.GetParameter (i).type) {
					case AnimatorControllerParameterType.Bool:
						log += mainAnimator.GetParameter (i).defaultBool.ToString ();
						break;
					case AnimatorControllerParameterType.Trigger:
						log += "Trigger has no init value";
						break;
					case AnimatorControllerParameterType.Int:
						log += mainAnimator.GetParameter (i).defaultInt.ToString ();

						break;
					case AnimatorControllerParameterType.Float:
						log += mainAnimator.GetParameter (i).defaultFloat.ToString ();
						break;
					}

					Debug.Log (log);
				}

				Debug.Log ("Transitions:");

				for (int i = 0; i < mainAnimator.layerCount; i++) {
					
				}
			}
		}

		private static void RecreatePropAssets() {

		}

		private static void RecreateProp(AssetTemplate tpl, Object prop) {
			if (prop == null) {
				Debug.Log ("Prop is null");
				return;
			}
				
			GameObject newGo = (GameObject)Instantiate (prop) as GameObject;
			newGo.name = tpl.Title;

			PropAsset asset = newGo.AddComponent<PropAsset> ();
			asset.SetTemplate (tpl);

			FixShaders (newGo);

			Transform thumbnails =  asset.GetLayer (HierarchyLayers.Thumbnails);
			foreach(Transform tb in thumbnails) {
				PropThumbnail thumbnail = tb.gameObject.AddComponent<PropThumbnail> ();
				FixShaders (thumbnail.Border);
				FixShaders (thumbnail.Corner);
			}

			List<Transform> pointers = new List<Transform> ();
            Transform[] children = newGo.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++) {
                Transform child = children[i];

                if (child.name.Equals(AssetBundlesSettings.THUMBNAIL_POINTER)) {
                    child.parent.gameObject.AddComponent<PropMeshThumbnail> ().Update();
					pointers.Add (child);
				}
			}

			foreach(Transform t in pointers) {
				DestroyImmediate (t.gameObject);
			}

            AssetBundleContentCloner.Clone(asset);

            //text component
            foreach (SerializedText textInfo in asset.GetComponentsInChildren<SerializedText>()) {

				if(textInfo.FontFileContent  != null && textInfo.FontFileContent.Length > 0) {
                   
					string assetFolderPath = AssetBundlesSettings.AssetBundlesPathFull + "/" + tpl.Title + "/";
					string fontsFolder = assetFolderPath + "Fonts/";
					string fullPath = fontsFolder + textInfo.FullFontName;

					if (!FolderUtils.IsFolderExists(assetFolderPath)) {
						FolderUtils.CreateAssetComponentsFolder(assetFolderPath);
					}

					if (!FolderUtils.IsFolderExists(fontsFolder)) {
						FolderUtils.CreateAssetComponentsFolder(fontsFolder);
					}

					SA.Common.Util.Files.WriteBytes (AssetBundlesSettings.AssetBundlesPath + "/" + tpl.Title + "/Fonts/" + textInfo.FullFontName, textInfo.FontFileContent);

					textInfo.Font = (Font)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Font));

				} else {
					Debug.Log("no font content");
				}
  
                var text =  textInfo.gameObject.AddComponent<RoomfulText>();
                text.Restore(textInfo);
                GameObject.DestroyImmediate(textInfo);
            }

			WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

		}

		private static void FixShaders(GameObject obj) {
			if (obj == null) {
				return;
			}

			var renderers = obj.GetComponentsInChildren<Renderer> ();

			foreach (Renderer r in renderers) {
				foreach(Material m in r.sharedMaterials) {
					if(m == null) { continue; }
					if (m.shader == null) { continue; }


					var shaderName = m.shader.name;
					var newShader = Shader.Find(shaderName);
					if(newShader != null){
						m.shader = newShader;
					} else {
						Debug.LogWarning("unable to refresh shader: "+shaderName+" in material "+m.name);
					}
				}
			}
		}
	}
}
