using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard {

	[ExecuteInEditMode]
	public class PropAsset : MonoBehaviour {

        public static event System.Action PropInstantieted = delegate { };
		
		[SerializeField] [HideInInspector]
		private AssetTemplate _Template;
		public float Scale = 1f;
		public bool DrawGizmos = true;


		public PropDisplayMode DisplayMode = PropDisplayMode.Normal;

		public Texture2D Icon;
		public Mesh Silhouette;


		private Bounds _Size = new Bounds (Vector3.zero, Vector3.zero);

        public bool IsInited = false;

		//--------------------------------------
		// Initialization
		//--------------------------------------

		void Awake () {
			FinalVisualisation ();
		}

        void Start() {
            
            if (!IsInited) {
                PropInstantieted();
            }

            IsInited = true;
        }
		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		public void Update () {
			PreliminaryVisualisation ();
			CheckhHierarchy ();
		}



		protected virtual void OnDrawGizmos () {

			if(!DrawGizmos) {
				return;
			}

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.04f);

            GizmosDrawer.DrawCube (_Size.center, transform.rotation, _Size.size, Color.cyan);

		}


		public static void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale) {
			Matrix4x4 cubeTransform = Matrix4x4.TRS (position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

			Gizmos.matrix *= cubeTransform;

			Gizmos.DrawWireCube (Vector3.zero, Vector3.one);

			Gizmos.matrix = oldGizmosMatrix;

		}


	
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		[ContextMenu("Prepare For Upload")]
		public void PrepareForUpload () {
			IPropComponent[] components = GetComponentsInChildren<IPropComponent> ();
			foreach(var c in components) {
				c.RemoveSilhouette ();
			}

			Scale = 1f;
			Template.Silhouette = SilhouetteMeshData;


			if(HasStandSurface) {
				Template.CanStack = false;
			}
				

			foreach(var c in components) {
				c.PrepareForUpalod ();
			}

			FinalizeUploadPreparation ();
		}

		public void FinalizeUploadPreparation() {
			DisplayMode = PropDisplayMode.Normal;
			DestroyImmediate (GetLayer (HierarchyLayers.Silhouette).gameObject);


            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                if (renderer != null) {

                    foreach (Material mat in renderer.sharedMaterials) {
						if (mat != null) {
							var md = renderer.gameObject.AddComponent<SerializedMaterial>();
							md.Serialize(mat);
						}
                   	}

                    renderer.sharedMaterials = new Material[0];
                }
            }
		}

		public void Refresh() {
			SetTemplate (Template);
		}


		public void SetTemplate (AssetTemplate tpl) {

			_Template = tpl;


			if (_Template.Silhouette != null && !string.IsNullOrEmpty(_Template.Silhouette.MeshData)) {

				GetLayer (HierarchyLayers.Silhouette).Clear ();

	
				GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				go.name = "Restored Silhouette Mesh";
				go.transform.parent = GetLayer (HierarchyLayers.Silhouette);
				go.transform.localPosition = Vector3.zero;


				DestroyImmediate (go.GetComponent<BoxCollider> ());

				byte[] bytesToEncode = System.Convert.FromBase64String (_Template.Silhouette.MeshData);
				go.GetComponent<MeshFilter> ().sharedMesh = MeshSerializer.ReadMesh (bytesToEncode);
				go.GetComponent<MeshFilter> ().sharedMesh.name = "Silhouette";  

			}
		}

		public Transform GetLayer (HierarchyLayers layer) {
            return GetLayer(layer.ToString()) ;
		}

        public Transform GetLayer(string layer) {
            Transform hLayer = Model.Find(layer);
            if (hLayer == null) {
                GameObject go = new GameObject(layer);
                go.transform.parent = Model;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;

                hLayer = go.transform;
            }

            return hLayer;
        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public AssetTemplate Template {
			get {
				if (_Template == null) {
					_Template = new AssetTemplate ();
				}

				return _Template;
			}
		}

		public Transform Model {
			get {
				Transform model = transform.Find ("Model");
				if (model == null) {
					GameObject go = new GameObject ("Model");
					go.transform.parent = transform;
					go.transform.localPosition = Vector3.zero;
					go.transform.localScale = Vector3.one;
					go.transform.localRotation = Quaternion.identity;

					model = go.transform;
				}

				return model;
			}
		}


		public GameObject Environment {
			get {

				var rig =  GameObject.Find ("Environment");
				if(rig == null) {
					rig = PrefabManager.CreatePrefab ("Environment");
				}

				rig.transform.SetSiblingIndex (0);
					
				return rig;
			}
		}

		public float MaxAxisValue {
			get {
				float val = Mathf.Max (Size.x, Size.y);
				return  Mathf.Max (val, Size.z);
			}
		}


		public Vector3 Size {
			get {
				return _Size.size / Scale;
			}
		}


		public float MaxScale {
			get {

				if (MaxAxisValue == 0) {
					return 1;
				}
				return Template.MaxSize / MaxAxisValue;
			}
		}

		public float MinScale {
			get {
				if (MaxAxisValue == 0) {
					return 1;
				}
				return Template.MinSize / MaxAxisValue;
			}
		}


		public bool HasStandSurface {
			get {
               
				if (gameObject.GetComponentsInChildren<SerializedFloorMarker>().Length != 0) {
					return true;
				}

				return false;
			}
		}



		public AssetSilhouette SilhouetteMeshData {
			get {


				Vector3 storedPos = transform.position;
				transform.position = Vector3.zero;

				var silhouette = new AssetSilhouette (this);

				transform.position = storedPos;

				return silhouette;
			}

		}


		//--------------------------------------
		// Private Methods
		//--------------------------------------


		private void PreliminaryVisualisation () {
			
			foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
				GetLayer (layer).gameObject.SetActive (true);
			}

			GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (false);
		}


		private void FinalVisualisation () {


			foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
				GetLayer (layer).gameObject.SetActive (true);


				Renderer[] silhouetteRenderers = GetLayer (HierarchyLayers.Silhouette).GetComponentsInChildren<Renderer> ();
				foreach (Renderer r in silhouetteRenderers) {
					if(r.gameObject.GetComponent<SilhouetteCustomMaterial>() != null) {
						continue;
					}

					if (r.sharedMaterial != null) {
						r.sharedMaterial = new Material (Shader.Find ("Roomful/Silhouette"));
					}
				}
			}

			switch(DisplayMode) {

			case PropDisplayMode.Normal:
				GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (false);
				break;
			case PropDisplayMode.Silhouette:
				foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
					if(layer != HierarchyLayers.Silhouette){
						GetLayer (layer).gameObject.SetActive (false);
					}
				}




				break;
			}

		}

		public void UpdateBounds () {

			bool hasBounds = false;

			_Size = new Bounds (Vector3.zero, Vector3.zero);
			Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer> ();

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			foreach (Renderer child in ChildrenRenderer) {
	
				if (IsIgnored(child.transform)) {
					continue;
				}

				if (child.transform.IsChildOf (GetLayer (HierarchyLayers.Silhouette))) {
					continue;
				}


				if (!hasBounds) {
					_Size = child.bounds;
					hasBounds = true;
				} else {
					_Size.Encapsulate (child.bounds);
				}
			}

			transform.rotation = oldRotation;

			Template.Size = _Size.size;
		}


        public bool IsIgnored(Transform go) {

            Transform testedObject = go;
            while(testedObject != null) {
                if (testedObject.GetComponent<SerializedBoundsIgnoreMarker>() != null) {
                    return true;
                }
                testedObject = testedObject.parent;
            }

     
            return false;
        }



		private void CheckhHierarchy () {


			if(Icon == null) {
				Icon = Template.Icon.Thumbnail;
			}


			Model.localPosition = Vector3.zero;
			Model.localScale = Vector3.one * Scale;
			Model.localRotation = Quaternion.identity;

			Environment.transform.parent = null;
			Environment.transform.position = Vector3.zero;
			Environment.transform.rotation = Quaternion.identity;
			Environment.transform.localScale = Vector3.one;



			List<Transform> UndefinedObjects = new List<Transform> ();

			//check undefined chields
			foreach (Transform child in transform) {
				if (child != Model) {
					UndefinedObjects.Add (child);
				}
			}


			foreach (Transform child in Model) {
				if (!AssetHierarchySettings.HierarchyLayers.Contains (child.name)) {
					UndefinedObjects.Add (child);
				}
			}


			//check undefined scene objects
			Transform[] allObjects = UnityEngine.Object.FindObjectsOfType<Transform> ();
			foreach (Transform child in allObjects) {
				if (child == transform) {
					continue;
				}

				if (child == Model) {
					continue;
				}

				if (child == Environment.transform) {
					continue;
				}

				if (child.parent != null) {
					continue;
				}
					
				UndefinedObjects.Add (child);
			}

			if (DisplayMode == PropDisplayMode.Silhouette) {

				foreach (Transform undefined in UndefinedObjects) {
					undefined.SetParent (GetLayer (HierarchyLayers.Silhouette));
                    undefined.localPosition = Vector3.zero;

                }
			} else {
				foreach (Transform undefined in UndefinedObjects) {
					undefined.SetParent(GetLayer (HierarchyLayers.Graphics));
                    undefined.localPosition = Vector3.zero;
                }
			}


			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			if(Template.Placing == Placing.Floor) {
				transform.position = Vector3.zero;

				Vector3 rendererPoint = transform.GetVertex (VertexX.Center, VertexY.Bottom, VertexZ.Center);
				Vector3 diff = transform.position - rendererPoint;
				transform.position += diff;

			}

			if(Template.Placing == Placing.Wall) {
				transform.position = new Vector3 (0, 1.5f, -1.5f);

				Vector3 rendererPoint = transform.GetVertex (VertexX.Center, VertexY.Center, VertexZ.Back);
				Vector3 diff = transform.position - rendererPoint;
				transform.position += diff;
			}


			UpdateBounds ();
			FinalVisualisation ();

		}




	}
}