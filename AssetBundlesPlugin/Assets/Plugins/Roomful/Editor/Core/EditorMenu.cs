using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public static class EditorMenu {

		//--------------------------------------
		//  Top Menu
		//--------------------------------------


		[MenuItem("Roomful/Asset Wizzard &w", false, 100)]
		public static void ShowWizzrd() {
			WindowManager.ShowWizard ();
		}


		[MenuItem("Roomful/Create/Text &t", false, 0)]
		public static void ShowWizzrd2() {
			AddTextComponent ();
		}

		[MenuItem("Roomful/Create/Thumbnail &m", false, 1)]
		public static void ShowWizzrd3() {
			SetAsThumbnail ();
		}



		[MenuItem("Roomful/Add Component/Mesh Thumbnail &#m", false, 2)]
		public static void ShowWizzrd5() {
			MarkAsThumbnail ();
		}

		[MenuItem("Roomful/Add Component/Border &#b", false, 3)]
		public static void ShowWizzrd4() {
			AddBorder ();
		}



		[MenuItem("Roomful/Add Component/Floor &#f", false, 4)]
		public static void ShowWizzrd6() {
			MarkAsStand ();
		}

		[MenuItem("Roomful/Add Component/Ignore Bounds &#i", false, 5)]
		public static void ShowWizzrd7() {
			IgnoreObjectBounds ();
		}




	








		//--------------------------------------
		//  Context Menu
		//--------------------------------------


		[MenuItem("GameObject/Roomful/Text", false, 0)]
		static void AddTextComponent () {
			var text = new GameObject ("Text").AddComponent<RoomfulText>();
			text.RectTransform.sizeDelta = new Vector2(1f, 1f / 5f); 

			text.transform.localRotation = Quaternion.Euler(0f, 180f, 0f); 

			Selection.activeObject = text.gameObject;
		}


		[MenuItem("GameObject/Roomful/Thumbnail", false, 1)]
		static void SetAsThumbnail () {
			GameObject Thumbnail = new GameObject ("Thumbnail");
			Thumbnail.AddComponent<PropThumbnail>();
			Thumbnail.transform.localScale = Vector3.one * 1.5f;
		}


	

		[MenuItem("GameObject/Roomful/Add Component/Border", false, 100)]
		static void AddBorder () {
			
		}



		[MenuItem("GameObject/Roomful/Add Component/Floor", false, 101)]
		static void MarkAsStand () {


			bool valid = IsValidPropGameobject();
			if(Selection.activeGameObject.GetComponent<BoxCollider>() == null) {
				valid = false;
				EditorUtility.DisplayDialog ("Error", "Object should have BoxCollider component", "Ok");
			}

			if(valid) {
                Selection.activeGameObject.AddComponent<RF.AssetBundles.Serialization.SerializedStandMarker>();  
			}
		}

		[MenuItem("GameObject/Roomful/Add Component/Mesh Thumbnail", false, 102)]
		static void MarkAsThumbnail () {
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



		[MenuItem("GameObject/Roomful/Add Component/Ignore Bounds", false, 103)]
		static void IgnoreObjectBounds () {
			bool valid = IsValidPropGameobject();
			if(valid) {

				Collider c = Selection.activeGameObject.GetComponent<Collider> ();
				if(c != null) {
					GameObject.DestroyImmediate (c);
				}

                Selection.activeGameObject.AddComponent<RF.AssetBundles.Serialization.SerializedBoundsIgnoreMarker>();
            }

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