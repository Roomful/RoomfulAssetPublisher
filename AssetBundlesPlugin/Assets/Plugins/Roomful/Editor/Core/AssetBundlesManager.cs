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
			p.PrepareForUpload ();
			DestroyImmediate (p);

			PropThumbnail[] thumbnails =  clone.GetComponentsInChildren<PropThumbnail> ();
			foreach(var t in thumbnails) {
				t.PrepareForUpalod ();
			}

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
			
			/*	if (asset.Template.Thumbnail == null) {
				EditorUtility.DisplayDialog ("Error", "Set Asset thumbnail!", "Ok");
				return false;
			}*/

			if (asset.Model.childCount < 1) {
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


		private static AssetBundle CurrentAssetBundle = null;

		public static void LoadAssetBundle(AssetTemplate prop) {
			EditorApplication.delayCall = () => {


				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

				string pl = EditorUserBuildSettings.activeBuildTarget.ToString();

				Network.Request.GetAssetUrl getAssetUrl = new RF.AssetWizzard.Network.Request.GetAssetUrl (prop.Id, pl);

				getAssetUrl.PackageCallbackText = (assetUrl) => {
					Network.Request.DownloadAsset loadAsset = new RF.AssetWizzard.Network.Request.DownloadAsset (assetUrl);

					loadAsset.PackageCallbackData = (loadCallback) => {
						string bundlePath = AssetBundlesSettings.AssetBundlesPathFull+"/"+prop.Title+"_"+pl;

						FolderUtils.WriteBytes(bundlePath, loadCallback);

						if(CurrentAssetBundle  != null) {
							CurrentAssetBundle.Unload(true);

							CurrentAssetBundle = null;
						}

						CurrentAssetBundle = AssetBundle.LoadFromFile(bundlePath);

						RecreateProp(prop, CurrentAssetBundle.LoadAsset<Object>(prop.Title));

						//assetBundle.Unload(false);
						AssetDatabase.DeleteAsset(bundlePath);
					};

					loadAsset.Send ();
				};

				getAssetUrl.Send();
			};
		}

		public static void ReUploadAsset(PropAsset prop) {


			if(!AssetBundlesManager.ValidateAsset(prop)) { return; 	}

			prop.SynchTemplate ();
			RF.AssetWizzard.Network.Request.UpdateAsset updateRequest = new RF.AssetWizzard.Network.Request.UpdateAsset (prop.Template);

			updateRequest.PackageCallbackText = (updateCalback) => {
				UploadAssetBundle(prop);

			};
			updateRequest.Send ();
		}

		public static void UploadAssets(PropAsset prop) {

			if(!AssetBundlesManager.ValidateAsset(prop)) { return; }

			prop.SynchTemplate ();
			Network.Request.CreateMetaData createMeta = new RF.AssetWizzard.Network.Request.CreateMetaData (prop.Template);
			createMeta.PackageCallbackText = (callback) => { 
				prop.Template.Id =  new AssetTemplate(callback).Id;
				UploadAssetBundle(prop);
			};

			createMeta.Send ();
		}

		public static void SavePrefab(PropAsset prop) {
			string path = AssetBundlesSettings.FULL_ASSETS_LOCATION + prop.Template.Title + ".prefab";
			Object prafabObject = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
			if(prafabObject ==  null) {

				GameObject newPrfab = PrefabUtility.CreatePrefab (path, prop.gameObject);
				PrefabUtility.ConnectGameObjectToPrefab (prop.gameObject, newPrfab);

			} else {
				PrefabUtility.ReplacePrefab(prop.gameObject, prafabObject, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
			}
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

	
			SavePrefab (asset);
	
			WindowManager.Wizzard.SiwtchTab(WizzardTabs.Wizzard);

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




		private static void UploadAssetBundle(PropAsset prop) {

			var getIconUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink_Thumbnail (prop.Template.Id);
			getIconUploadLink.PackageCallbackText = (linkCallback) => {

				var uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset_Thumbnail(linkCallback, prop.Icon);
				uploadRequest.PackageCallbackText = (string uploadCallback)=> {


					var confirmRequest = new Network.Request.UploadConfirmation_Thumbnail(prop.Template.Id);
					confirmRequest.PackageCallbackText = (string resData)=> {
						

						var resInfo =  new JSONData(resData);
						var res = new Resource(resInfo);

						prop.Template.Icon = res;
						AssetBundlesSettings.Instance.ReplaceTemplate(prop.Template);


						SavePrefab(prop);

						AssetBundlesManager.Clone(prop);
						int counter = 0;

						AssetBundlesManager.AssetsUploadLoop(counter, prop.Template, () => {
							AssetBundlesManager.DelteTempFiles();
							AssetDatabase.Refresh();
							AssetDatabase.SaveAssets();

							EditorApplication.delayCall = () => {
								FolderUtils.DeleteFolder(AssetBundlesSettings.AssetBundlesPath, false);
								FolderUtils.CreateFolder(AssetBundlesSettings.AssetBundlesPath);
							};
						});


						EditorUtility.DisplayDialog ("Success", " Asset has been successfully uploaded!", "Ok");



					};
					confirmRequest.Send();
				};
				uploadRequest.Send();
			};
			getIconUploadLink.Send();


		}



	}

}
