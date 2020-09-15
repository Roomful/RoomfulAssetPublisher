using System.Collections.Generic;
using net.roomful.api;
using UnityEngine;
using net.roomful.assets.serialization;
using StansAssets.Foundation.Extensions;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal sealed class PropAsset : Asset<PropAssetTemplate>
    {
        [SerializeField] private float m_scale = 1f;

        private Bounds m_bounds = new Bounds(Vector3.zero, Vector3.zero);

        public float Scale {
            get => m_scale;
            set => m_scale = value;
        }

        //--------------------------------------
        // Unity Editor
        //--------------------------------------

        public void Update() {
            CheckHierarchy();
        }

        private void OnDrawGizmos() {
            if (!DrawGizmos) {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.04f);

            GizmosDrawer.DrawCube(m_bounds.center, transform.rotation, m_bounds.size, Color.cyan);
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        [ContextMenu("Prepare For Upload")]
        public override void PrepareForUpload() {
            m_scale = 1f;
            if (HasStandSurface) {
                Template.CanStack = false;
            }

            PrepareComponentsForUpload();
        }

        public void SetTemplate(PropAssetTemplate tpl) {
            _Template = tpl;
        }

        public Transform GetLayer(HierarchyLayers layer) {
            var hLayer = Model.Find(layer.ToString());
            if (hLayer == null) {
                var go = new GameObject(layer.ToString());
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

        private Transform Model {
            get {
                var model = transform.Find("Model");
                if (model == null) {
                    var go = new GameObject("Model");
                    go.transform.parent = transform;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.transform.localRotation = Quaternion.identity;

                    model = go.transform;
                }

                return model;
            }
        }

        public IEnumerable<Animator> AnimatorControllers => GetComponentsInChildren<Animator>();

        public float MaxAxisValue {
            get {
                var val = Mathf.Max(Size.x, Size.y);
                return Mathf.Max(val, Size.z);
            }
        }

        public Vector3 Size => m_bounds.size / m_scale;

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

                return Mathf.Clamp(Template.MinSize / MaxAxisValue, 0.1f, 1);
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

        public bool HasCollision {
            get {
                var colliders = GetComponentsInChildren<Collider>();
                var thumbnails = GetComponentsInChildren<PropThumbnail>();

                if (colliders.Length == 0 && thumbnails.Length == 0) {
                    return false;
                }

                return true;
            }
        }

        public bool ValidSize => true;

        private PropBounds m_boundsManager = null;

        private PropBounds BoundsManager {
            get {
                if (m_boundsManager == null) {
                    m_boundsManager = new PropBounds();
                }

                return m_boundsManager;
            }
        }

        public IEnumerable<Renderer> Renderers => GetComponentsInChildren<Renderer>();

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private void UpdateBounds() {
            m_bounds = BoundsManager.Calculate(gameObject);
            Template.Size = m_bounds.size;
        }

        protected override void CheckHierarchy() {
            base.CheckHierarchy();

            Model.Reset();
            Environment.transform.parent = null;
            Environment.transform.Reset();

            var undefinedObjects = new List<Transform>();

            //check undefined children
            foreach (Transform child in transform) {
                if (child != Model) {
                    undefinedObjects.Add(child);
                }
            }

            foreach (Transform child in Model) {
                if (!AssetHierarchySettings.HierarchyLayers.Contains(child.name)) {
                    undefinedObjects.Add(child);
                }
            }

            //check undefined scene objects
            var allObjects = FindObjectsOfType<Transform>();
            foreach (var child in allObjects) {
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

                undefinedObjects.Add(child);
            }

            foreach (var undefined in undefinedObjects) {
                undefined.SetParent(GetLayer(HierarchyLayers.Graphics));
                undefined.localPosition = Vector3.zero;
            }

            var propTransform = transform;
            propTransform.rotation = Quaternion.identity;
            propTransform.localScale = Vector3.one * m_scale;

            if (Template.Placing == PlacingType.Floor) {
                transform.position = Vector3.zero;

                var rendererPoint = propTransform.GetVertex(SA_VertexX.Center, SA_VertexY.Bottom, SA_VertexZ.Center);
                var diff = propTransform.position - rendererPoint;
                propTransform.position += diff;
            }

            if (Template.Placing == PlacingType.Wall) {
                propTransform.position = new Vector3(0, 1.5f, -1.5f);

                var rendererPoint = transform.GetVertex(SA_VertexX.Center, SA_VertexY.Center, SA_VertexZ.Back);
                var diff = propTransform.position - rendererPoint;
                propTransform.position += diff;
            }

            UpdateBounds();
        }
    }
}