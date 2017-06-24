using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	#endif

	public class PropThumbnailPointer : MonoBehaviour, PropComponent {


		public int ImageIndex = 0;
		public Texture2D Thumbnail;

		//--------------------------------------
		// Initialisaction
		//--------------------------------------

		void Awake() {
			Thumbnail = Resources.Load ("logo_square") as Texture2D;
		}



		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		#if UNITY_EDITOR
	
		public void Update() {

			gameObject.GetComponent<Renderer> ().sharedMaterial = new Material (gameObject.GetComponent<Renderer> ().sharedMaterial);
			gameObject.GetComponent<Renderer> ().sharedMaterial.mainTexture = Thumbnail;
			GenerateSilhouette ();
		}

		#endif

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void PrepareForUpalod() {

			gameObject.GetComponent<Renderer> ().sharedMaterial.mainTexture = null;

			GameObject pointer = new GameObject (AssetBundlesSettings.THUMBNAIL_POINTER);
			pointer.transform.parent = transform;
			pointer.transform.Reset ();
			DestroyImmediate (Silhouette.gameObject);

			DestroyImmediate (this);
		}


	

	


		//--------------------------------------
		// Public Methods
		//--------------------------------------


	
		public Transform Silhouette {
			get {
				Transform silhouette = Prop.GetLayer (HierarchyLayers.Silhouette).Find (gameObject.GetInstanceID ().ToString());
				if(silhouette == null) {
					silhouette = new GameObject (gameObject.GetInstanceID ().ToString()).transform;
					silhouette.parent = Prop.GetLayer (HierarchyLayers.Silhouette);
				}

				silhouette.localPosition = transform.localPosition;
				silhouette.localRotation = transform.localRotation;
				silhouette.localScale = transform.localScale;

				return silhouette;
			}
		}

		public PropAsset Prop {
			get {
				return GameObject.FindObjectOfType<PropAsset> ();
			}
		}






		//--------------------------------------
		// Private Methods
		//--------------------------------------

	

		private void GenerateSilhouette() {
			Silhouette.Clear ();

			Vector3 storedPos = Prop.transform.position;
			Prop.transform.position = Vector3.zero;

			GameObject silhouette = Instantiate (gameObject) as GameObject;
			silhouette.transform.parent = Silhouette;


			Prop.transform.position = storedPos;
		}



	}



}
