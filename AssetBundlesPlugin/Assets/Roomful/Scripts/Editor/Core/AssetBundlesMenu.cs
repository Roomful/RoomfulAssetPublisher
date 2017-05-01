using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public static class AssetBundlesMenu {

		[MenuItem("Roomful/Asset Wizzard &w", false, 0)]
		//[MenuItem("Roomful/Asset Wizzard %w", false, 400)]
		public static void ShowWizzrd() {


			EditorWindow window = EditorWindow.GetWindow<WizzardWindow>(true, "Asset Bundles Wizzard");

			window.minSize = new Vector2(600f, 400f);
			window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
			window.position = new Rect(new Vector2(100f, 100f), window.minSize);
			window.Show();

		}


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