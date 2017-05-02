using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public static class AssetBundlesMenu {



		//--------------------------------------
		//  Top Menu
		//--------------------------------------


		[MenuItem("Roomful/Asset Wizzard &w", false, 0)]
		public static void ShowWizzrd() {
			WindowManager.ShowWizard ();
		}



		//--------------------------------------
		//  Context Menu
		//--------------------------------------


		[MenuItem("GameObject/Roomful/Mark As Stand Surface", false, 0)]
		static void MarkAsStand () {

			var prop = GameObject.FindObjectOfType<PropAsset> ();
	
			bool valid = true;
			if(prop == null) {
				EditorUtility.DisplayDialog ("Error", "No valid prop found", "Ok");
				valid = false;
			}

			if(prop.gameObject == Selection.activeGameObject) {
				EditorUtility.DisplayDialog ("Error", "Select a child object", "Ok");
				valid = false;
			}

			if(Selection.activeGameObject.GetComponent<BoxCollider>() == null) {
				valid = false;
				EditorUtility.DisplayDialog ("Error", "Object should have BoxCollider component", "Ok");
			}
				

			if(valid) {
				Selection.activeGameObject.transform.parent = prop.GetLayer(HierarchyLayers.StandSurface);
			}
				
		}


		[MenuItem("GameObject/Roomful/Attch Thumbnail Component", false, 0)]
		static void SetAsThumbnail () {

		}

		[MenuItem("GameObject/Roomful/Attch Title Component", false, 0)]
		static void SetAsTitle () {

		}


	}
}