using net.roomful.api;
using UnityEngine;
using net.roomful.assets.serialization;
using StansAssets.Foundation.Extensions;

namespace net.roomful.assets
{

    [ExecuteInEditMode]
    class StylePanel : MonoBehaviour
    {

        public const string WallParentName = "Wall";
        public const string FloorParentName = "Floor";
        public const string CeilingParentName = "Ceiling";

        Bounds m_Bounds = new Bounds(Vector3.zero, Vector3.zero);
        
        //--------------------------------------
        // Unity Editor
        //--------------------------------------

        void Awake() {
            IconRenderer.hideFlags = HideFlags.HideInInspector;
        }
        
        void Update() {

            if(Style != null) {
                transform.parent = Style.transform;
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
            GizmosDrawer.DrawCube(m_Bounds.center, transform.rotation, m_Bounds.size, Color.cyan);

#if UNITY_EDITOR
            var style = new GUIStyle();
            style = UnityEditor.EditorStyles.boldLabel;

            var pos = Bounds.GetVertex(SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexZ.Front);
            UnityEditor.Handles.Label(pos, transform.gameObject.name, style);
#endif

        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void SetPanelBoundsCenter(Vector3 pos) {
            var diff = transform.position - Bounds.GetVertex(SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexZ.Front);
            var newPos = pos + (Style.Template.StyleType == StyleType.NoMirror ? new Vector3(diff.x, diff.y, 0.0f) : diff);
            newPos += new Vector3(0.01f, 0f, 0f);

            //no point to move the panel, it's already placed where it should be
            var posError = transform.position - newPos;
            if (posError.magnitude < 0.01) {
                return;
            }

            transform.position = newPos;
            UpdateBounds();
        }


        public void PrepareForUpload() {
            IconRenderer.sharedMaterial.name = name + "_icon";
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        StyleAsset m_style = null;

        StyleAsset Style {
            get {
                if(m_style == null) {
                    m_style = FindObjectOfType<StyleAsset>();
                }

                return m_style;
            }
        }

        public Transform Wall => GetPanelPart(WallParentName);

        public Transform Floor => GetPanelPart(FloorParentName);

        public Transform Ceiling => GetPanelPart(CeilingParentName);

        public Bounds Bounds => m_Bounds;

        BoxCollider PanelCollider {
            get {
                var collider = gameObject.GetComponent<BoxCollider>();
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
                DestroyImmediate(IconRenderer);
                IconRenderer.sharedMaterial.mainTexture = value;
            }
        }

        public bool IsFirstPanel => transform.GetSiblingIndex() == 0;

        public bool IsLastPanel => transform.GetSiblingIndex() == (transform.parent.childCount - 1);

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

        PanelBounds m_boundsManager = null;

        PanelBounds BoundsManager {
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

        Transform GetPanelPart(string partName) {

            var part = transform.Find(partName);
            if (part == null) {
                part = new GameObject(partName).transform;
                part.parent = transform;
            }

            return part;
        }

        void UpdateBounds() {
            m_Bounds = BoundsManager.Calculate(gameObject);

            PanelCollider.center = m_Bounds.center - transform.position;
            PanelCollider.size = m_Bounds.size;
        }
    }
}