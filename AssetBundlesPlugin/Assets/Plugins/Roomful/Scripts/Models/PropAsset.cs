using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard {

	[ExecuteInEditMode]
	public class PropAsset : Asset<PropTemplate>
    {

        public static event System.Action PropInstantieted = delegate { };
		
		public float Scale = 1f;
		
        public bool IsInited = false;

        public PropDisplayMode DisplayMode = PropDisplayMode.Normal;
		public Mesh Silhouette;


		private Bounds m_bounds = new Bounds (Vector3.zero, Vector3.zero); 

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

            GizmosDrawer.DrawCube (m_bounds.center, transform.rotation, m_bounds.size, Color.cyan);

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
        public override void PrepareForUpload() {

            CleanUpSilhouette();

            Scale = 1f;
			Template.Silhouette = SilhouetteMeshData;

            if (HasStandSurface) {
				Template.CanStack = false;
			}


            DisplayMode = PropDisplayMode.Normal;
            DestroyImmediate(GetLayer(HierarchyLayers.Silhouette).gameObject);

            PrepareCoponentsForUpload();
		}


		public void Refresh() {
			SetTemplate (Template);
		}


		public void SetTemplate (PropTemplate tpl) {

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


		

		public float MaxAxisValue {
			get {
				float val = Mathf.Max (Size.x, Size.y);
				return  Mathf.Max (val, Size.z);
			}
		}


		public Vector3 Size {
			get {
				return m_bounds.size / Scale;
			}
		}


		public float MaxScale {
			get {

				if (MaxAxisValue == 0) {
					return 1;
				}

                return Mathf.Clamp(Template.MaxSize / MaxAxisValue, 1f, 2f);
            }
		}

		public float MinScale {
			get {
				if (MaxAxisValue == 0) {
					return 1;
				}


				return  Mathf.Clamp(Template.MinSize / MaxAxisValue, 0.1f, 1);
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

        public bool HasCollisison {
            get {
                Collider[] colliders = GetComponentsInChildren<Collider>();
                PropThumbnail[] thumbnails = GetComponentsInChildren<PropThumbnail>();

                if (colliders.Length == 0 && thumbnails.Length == 0) {
                    return false;
                }

                return true;
            }
        }

        public bool ValidSize {
            get {

                //that's disabled so far
                return true;
                /*
                if (MaxAxisValue > PropTemplate.MAX_ALLOWED_AXIS_SIZE || MaxAxisValue < PropTemplate.MIN_ALLOWED_AXIS_SIZE) {
                    return false;
                } else {
                    return true;
                }
                */
            }
        }

        private PropBounds m_boundsManager = null;
        private PropBounds BoundsManager {
            get {
                if(m_boundsManager ==  null) {
                    m_boundsManager = new PropBounds();
                    m_boundsManager.SetSilhouetteLayer(GetLayer(HierarchyLayers.Silhouette));
                }

                return m_boundsManager;
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

		private void UpdateBounds () {
            m_bounds = BoundsManager.Calculate(gameObject);
			Template.Size = m_bounds.size;
		}


       



		protected override void CheckhHierarchy () {

            base.CheckhHierarchy();

            Model.Reset();
            Environment.transform.parent = null;
            Environment.transform.Reset(); 


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
			transform.localScale = Vector3.one * Scale;

			if(Template.Placing == Placing.Floor) {
				transform.position = Vector3.zero;

				Vector3 rendererPoint = transform.GetVertex (SA_VertexX.Center, SA_VertexY.Bottom, SA_VertexZ.Center);
				Vector3 diff = transform.position - rendererPoint;
				transform.position += diff;

			}

			if(Template.Placing == Placing.Wall) {
				transform.position = new Vector3 (0, 1.5f, -1.5f);

				Vector3 rendererPoint = transform.GetVertex (SA_VertexX.Center, SA_VertexY.Center, SA_VertexZ.Back);
				Vector3 diff = transform.position - rendererPoint;
				transform.position += diff;
			}


			UpdateBounds ();
			FinalVisualisation ();
		}
    }
}