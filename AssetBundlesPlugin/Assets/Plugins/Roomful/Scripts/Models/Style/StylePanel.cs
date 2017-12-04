using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class StylePanel : MonoBehaviour
    {

        public const string WALL_PARENT_NAME = "Wall";
        public const string FLOOR_PARENT_NAME = "Floor";
        public const string CEILING_PARENT_NAME = "Ceiling";


        private Bounds m_bounds = new Bounds(Vector3.zero, Vector3.zero);



        //--------------------------------------
        // Unity Editor
        //--------------------------------------

        void Awake() {
            IconRenderer.hideFlags = HideFlags.HideInInspector;
        }


        void Update() {
            var style = GameObject.FindObjectOfType<StyleAsset>();
            if(style != null) {
                transform.parent = style.transform;
            }

            Wall.Reset();
            Floor.Reset();
            Ceiling.Reset();

            UpdateBounds();

            Settings.PanelName = name;

        }

        void OnDrawGizmos() {

            if (Scene.ActiveAsset == null || !Scene.ActiveAsset.DrawGizmos) {
                return;
            }

            Gizmos.color = Color.green;
            GizmosDrawer.DrawCube(m_bounds.center, transform.rotation, m_bounds.size, Color.cyan);

#if UNITY_EDITOR
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            style = UnityEditor.EditorStyles.boldLabel;

            Vector3 pos = Bounds.GetVertex(VertexX.Left, VertexY.Bottom, VertexZ.Front);
            UnityEditor.Handles.Label(pos, transform.gameObject.name, style);
#endif

        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void SetPosition(Vector3 pos, bool animated = true) {

            Vector3 diff = transform.position - Bounds.GetVertex(VertexX.Left, VertexY.Bottom, VertexZ.Front);// LeftBorderVertex;
            Vector3 newPos = pos + diff;
            newPos += new Vector3(0.01f, 0f, 0f);


            //no point to move the panel, it's already plased where it should be
            Vector3 posError = transform.position - newPos;
            if (posError.magnitude < 0.01) {
                return;
            }

            transform.position = newPos;
            UpdateBounds();
        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public Transform Wall {
            get {
               return GetPanelPart(WALL_PARENT_NAME);
            }
        }

        public Transform Floor {
            get {
                return GetPanelPart(FLOOR_PARENT_NAME);
            }
        }

        public Transform Ceiling {
            get {
                return GetPanelPart(CEILING_PARENT_NAME);
            }
        }

        public Bounds Bounds {
            get {
                return m_bounds;
            }
        }


        private BoxCollider PanelCollider {
            get {
                BoxCollider collider = gameObject.GetComponent<BoxCollider>();
                if (collider == null) {
                    collider = gameObject.AddComponent<BoxCollider>();
                }

                return collider;
            }
        }

        public MeshRenderer IconRenderer {
            get {

                var renderer = GetComponent<MeshRenderer>();
                if (renderer == null) {
                    renderer = gameObject.AddComponent<MeshRenderer>();
                }

                if(renderer.sharedMaterial == null) {
                    renderer.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
                    renderer.sharedMaterial.name = name + "_icon";
                }  

                renderer.hideFlags = HideFlags.HideInInspector;
                return renderer;
            }
        }

        public Texture2D Icon {
            get {
                if(IconRenderer.sharedMaterial == null) {
                    return null;
                }
                return (Texture2D)IconRenderer.sharedMaterial.mainTexture;
            } set {

                IconRenderer.sharedMaterial.mainTexture = value;
            }
        }

        public bool IsFirstPanel {
            get {
               return transform.GetSiblingIndex() == 0;
            }
        }

        public bool IsLastPanel {
            get {
                return transform.GetSiblingIndex() == (transform.parent.childCount - 1);
            }
        }


        public SerializedStylePanel Settings {
            get {

                var settings = GetComponent<SerializedStylePanel>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedStylePanel>();
                }

                settings.hideFlags = HideFlags.HideInInspector;

                return settings;
            }
        }


        private PanelBounds m_boundsManager = null;
        private PanelBounds BoundsManager {
            get {
                if (m_boundsManager == null) {
                    m_boundsManager = new PanelBounds();
                }

                return m_boundsManager;
            }
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private Transform GetPanelPart(string partName) {

            var part = transform.Find(partName);
            if (part == null) {
                part = new GameObject(partName).transform;
                part.parent = transform;
            }

            return part;
        }

        private void UpdateBounds() {
            m_bounds = BoundsManager.Calculate(gameObject);

            PanelCollider.center = m_bounds.center - transform.position;
            PanelCollider.size = m_bounds.size;
        }
    }
}