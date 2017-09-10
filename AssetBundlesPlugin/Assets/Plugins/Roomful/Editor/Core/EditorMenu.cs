using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public static class EditorMenu {

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
	
			bool valid = IsValidPropGameobject();
			if(Selection.activeGameObject.GetComponent<BoxCollider>() == null) {
				valid = false;
				EditorUtility.DisplayDialog ("Error", "Object should have BoxCollider component", "Ok");
			}
				

			if(valid) {
				Selection.activeGameObject.transform.parent = prop.GetLayer(HierarchyLayers.StandSurface);
			}
		}


		[MenuItem("GameObject/Roomful/Mark As Thumbnail", false, 0)]
		static void MarkAsThumbnail () {
			//var prop = GameObject.FindObjectOfType<PropAsset> ();
			bool valid = IsValidPropGameobject();

			if(valid) {
				Renderer r = Selection.activeGameObject.GetComponent<Renderer> ();
				if(r ==  null) {
					EditorUtility.DisplayDialog ("Error", "Object should have Renderer component", "Ok");
					return;
				}

				if(Selection.activeGameObject.GetComponent<PropMeshThumbnail>() != null) {
					EditorUtility.DisplayDialog ("Error", "PropThumbnailPointer", "Ok");
					return;
				}

				Selection.activeGameObject.AddComponent<PropMeshThumbnail> ().Update();

			}
		}


		[MenuItem("GameObject/Roomful/Ignore Object Bounds", false, 0)]
		static void IgnoreObjectBounds () {

			var prop = GameObject.FindObjectOfType<PropAsset> ();
			bool valid = IsValidPropGameobject();
			if(valid) {

				Collider c = Selection.activeGameObject.GetComponent<Collider> ();
				if(c != null) {
					GameObject.DestroyImmediate (c);
				}

				Selection.activeGameObject.transform.parent = prop.GetLayer(HierarchyLayers.IgnoredGraphics);
			}

		}


		[MenuItem("GameObject/Roomful/Add Thumbnail Component", false, 0)]
		static void SetAsThumbnail () {
			GameObject Thumbnail = new GameObject ("Thumbnail");
			Thumbnail.AddComponent<PropThumbnail>();
			Thumbnail.transform.localScale = Vector3.one * 1.5f;
		}



		[MenuItem("GameObject/Roomful/Add Title Component", false, 0)]
		static void SetAsTitle () {

		}


		private static bool IsValidPropGameobject() {
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

			return valid;

		}

	}
}