using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;




namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class PropTiledFrame : AbstractPropFrame {
        private GameObject Filler;
        private const string FILLER_NAME = "Filler";

        protected override void Awake() {
            base.Awake();
            CreateFiller();
        }

        private void CreateFiller() {
            Filler = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Filler.name = FILLER_NAME;
            Filler.gameObject.SetActive(false);
            Filler.transform.parent = GetLayer(BorderLayers.BorderParts);
            var fillerRenderer = Filler.GetComponent<Renderer>();
            if (fillerRenderer != null) {
                fillerRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
                fillerRenderer.sharedMaterial.SetColor("_Color", Settings.FillerColor);
            }
        }

        private Vector3 m_borderPartSize;
        private Vector3 m_cornerPartSize;
        private Vector2 m_canvasSize;
        private Vector2 m_fillerSize;
        private Vector3 m_fillerOffset;
        private Vector2 m_tilesOffset;
        private Vector2 m_borderPartCount;
        private bool m_frameIsInvalid;
        private float m_borderCornerHeightDiff;

        private void CalculateDimentions() {
            m_canvasSize = Bounds.size;
            if (m_frameIsInvalid) {
                m_borderPartSize = Vector3.zero;
                m_cornerPartSize = Vector3.zero;
                m_borderPartCount = Vector2.zero;
                m_fillerSize = m_canvasSize;
                m_fillerOffset = Vector3.zero;
                m_tilesOffset = Vector2.zero;
            }
            else {
                GameObject border = InstantiateBorderPart(Border.gameObject, true);
                m_borderPartSize = border.GetRendererBounds().size;
                DestroyImmediate(border);
                GameObject corner = InstantiateBorderPart(Corner.gameObject, true);
                m_cornerPartSize = corner.GetRendererBounds().size;
                DestroyImmediate(corner);
                int tilesCountHorizontal = Mathf.CeilToInt(m_canvasSize.x / m_borderPartSize.x);
                int tilesCountVertical = Mathf.CeilToInt(m_canvasSize.y / m_borderPartSize.x);
                var tiledSquare = new Vector2(tilesCountHorizontal * m_borderPartSize.x, tilesCountVertical * m_borderPartSize.x);
                m_borderPartCount = new Vector2(tilesCountHorizontal, tilesCountVertical);
                m_borderCornerHeightDiff =  m_cornerPartSize.x - m_borderPartSize.y;
                m_fillerSize = new Vector2(tiledSquare.x + 2 * m_borderCornerHeightDiff, tiledSquare.y + 2 * m_borderCornerHeightDiff);
                m_fillerOffset = new Vector3((m_fillerSize.x - m_canvasSize.x) / 2, (m_fillerSize.y - m_canvasSize.y) / 2, -0.0001f);
                m_tilesOffset = new Vector2((tiledSquare.x - m_canvasSize.x) / 2, (tiledSquare.y - m_canvasSize.y) / 2);
            }
        }


        public SerializedTiledFrame Settings {
            get {

                var settings = GetComponent<SerializedTiledFrame>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedTiledFrame>();
                }

                settings.hideFlags = HideFlags.None;

                return settings;
            }
        }

        protected override void CheckhHierarchy() {
            m_frameIsInvalid = Corner == null || Border == null;

            var borderParts = GetLayer(BorderLayers.BorderParts);

            if (Corner != null) {
                if (IsPersistent(Corner) || Corner.scene != gameObject.scene) {
                    Corner = Instantiate(Corner);
                }
                    
                

                Corner.transform.parent = borderParts;
                Corner.SetActive(false);
                Corner.gameObject.name = CORNER_NAME;

                if (Corner == Border) { Border = null; }
            }

            if (Border != null) {
                if (IsPersistent(Border) || Border.scene != gameObject.scene) {
                    Border = Instantiate(Border);
                }

                Border.transform.parent = borderParts;
                Border.SetActive(false);
                Border.gameObject.name = BORDER_NAME;
            }

            if (Filler != null) {
                Filler.transform.parent = borderParts;
                Filler.SetActive(false);
                Filler.name = FILLER_NAME;
                var renderer = Filler.GetComponent<Renderer>();
                if (renderer != null) {
                    renderer.sharedMaterial.SetColor("_Color", Settings.FillerColor);
                }
            }

            if (Back != null) {

                Back.transform.parent = borderParts; 
                Back.gameObject.SetActive(false);
                Back.gameObject.name = BACK_NAME;
            }

            Transform[] parts = borderParts.GetComponentsInChildren<Transform>(true);
            foreach(Transform part in parts) {
                GameObject go = part.gameObject;
                if(go != Border && go != Corner && go != Back && go != Filler && go != borderParts.gameObject) {
                    DestroyImmediate(go);
                }
            }
        }

        private static PropAsset CurrentProp {
            get {
                return GameObject.FindObjectOfType<PropAsset>();
            }
        }

        private GameObject InstantiateBorderPart(GameObject reference, bool resetScale = false) {
            GameObject p = Instantiate<GameObject>(reference, GetLayer(BorderLayers.GeneratedBorder));
            p.SetActive(true);
            resetScale = false;
            if (resetScale) {
                p.transform.localScale = Vector3.one;
            } else {
                p.transform.localScale = p.transform.localScale / CurrentProp.Scale;
            }
            p.transform.localRotation = Quaternion.identity;
            return p;
        }

        private void SnapObjectToCanvas(GameObject obj, SA_VertexX canvasVertexHorizontal, SA_VertexY canvasVertexVertical, SA_VertexX objectVertexHorizontal, SA_VertexY objectVertexVertical, Vector3 offset) {
            obj.transform.position = Bounds.GetVertex(canvasVertexHorizontal, canvasVertexVertical, SA_VertexZ.Front);

            Vector3 rendererPoint = obj.GetVertex(objectVertexHorizontal, objectVertexVertical, SA_VertexZ.Back);
            Vector3 diff = obj.transform.position - rendererPoint;
            obj.transform.position += diff ;
            obj.transform.position += offset;

            Vector3 localPos = obj.transform.localPosition;
            localPos.z += (Settings.FrameOffset / CurrentProp.Scale);
            obj.transform.localPosition = localPos;
        }

        public override void SetBackOffset(float offset) {
            Settings.BackOffset = offset;
        }


        protected override void GenerateFrame() {
            Quaternion oldRotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            RemoveOldBorder();

            CalculateDimentions();

            if (!m_frameIsInvalid) {
                GenerateCorners();
                GenerateTiledBorders();
            }
            GenerateFiller();
            GenerateBack();

            transform.rotation = oldRotation;
        }

        private GameObject m_generatedFiller;

        private void GenerateBack() {
            if (Back != null) {
                GameObject back = InstantiateBorderPart(Back.gameObject);
                var backBounds = back.GetRendererBounds().size;
                float scaleX = backBounds.x / m_fillerSize.x;
                float scaleY = backBounds.y / m_fillerSize.y;

                Vector3 newScale = new Vector3(back.transform.localScale.x / scaleX, back.transform.localScale.y / scaleY, back.transform.localScale.z);
                back.transform.localScale = newScale;

                if (m_generatedFiller != null) {
                    back.transform.position = m_generatedFiller.GetVertex(SA_VertexX.Left, SA_VertexY.Top, SA_VertexZ.Back);
                }
                else {
                    back.transform.position = Bounds.GetVertex(SA_VertexX.Left, SA_VertexY.Top, SA_VertexZ.Back) + m_fillerOffset;
                }


                Vector3 rendererPoint = back.GetVertex(SA_VertexX.Left, SA_VertexY.Top, SA_VertexZ.Front);
                Vector3 diff = back.transform.position - rendererPoint;
                back.transform.position += diff;

                Vector3 localPos = back.transform.localPosition;
                localPos.z += (Settings.BackOffset / Prop.Scale);
                back.transform.localPosition = localPos;

            }
        }

        private void RemoveOldBorder() {
            Transform GeneratedBorder = GetLayer(BorderLayers.GeneratedBorder);
            var children = new List<GameObject>();
            foreach (Transform child in GeneratedBorder)
                children.Add(child.gameObject);
            children.ForEach(child => DestroyImmediate(child));
        }

        private void GenerateTiledBorders() {

            for (var i = 0; i < m_borderPartCount.x; i++) {
                var tBorder = InstantiateBorderPart(Border.gameObject, true);
                tBorder.transform.Rotate(Vector3.forward, 180);
                var offset = new Vector3(m_tilesOffset.x - i * m_borderPartSize.x, m_fillerOffset.y);
                SnapObjectToCanvas(tBorder, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Bottom, offset);
                offset = new Vector3(m_tilesOffset.x - i * m_borderPartSize.x, -m_fillerOffset.y);
                var bBorder = InstantiateBorderPart(Border.gameObject, true);
                SnapObjectToCanvas(bBorder, SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexX.Left, SA_VertexY.Top, offset);
            }

            for (var i = 0; i < m_borderPartCount.y; i++) {
                var offset = new Vector3(-m_fillerOffset.x, m_tilesOffset.y - i * m_borderPartSize.x);
                var rBorder = InstantiateBorderPart(Border.gameObject, true);
                rBorder.transform.Rotate(Vector3.forward, 270);
                SnapObjectToCanvas(rBorder, SA_VertexX.Right, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Top, offset);
                var lBorder = InstantiateBorderPart(Border.gameObject, true);
                lBorder.transform.Rotate(Vector3.forward, 90);
                offset = new Vector3(m_fillerOffset.x, m_tilesOffset.y - i * m_borderPartSize.x);
                SnapObjectToCanvas(lBorder, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Right, SA_VertexY.Top, offset);
            }
        }

        private void GenerateFiller() {
            if (Filler != null) {
                m_generatedFiller = InstantiateBorderPart(Filler.gameObject);
                var fillerBounds = m_generatedFiller.GetRendererBounds().size;
                float scaleX = fillerBounds.x / m_fillerSize.x;
                float scaleY = fillerBounds.y / m_fillerSize.y;

                Vector3 newScale = new Vector3(m_generatedFiller.transform.localScale.x / scaleX, m_generatedFiller.transform.localScale.y / scaleY, 0.001f);
                m_generatedFiller.transform.localScale = newScale;
                m_generatedFiller.transform.position = Bounds.GetVertex(SA_VertexX.Left, SA_VertexY.Top, SA_VertexZ.Back) + m_fillerOffset;

                Vector3 rendererPoint = m_generatedFiller.GetVertex(SA_VertexX.Left, SA_VertexY.Top, SA_VertexZ.Front);
                Vector3 diff = m_generatedFiller.transform.position - rendererPoint;
                m_generatedFiller.transform.position += diff;

            }
            else {
                m_generatedFiller = null;
            }
        }

        private GameObject InstantiateCorner(float rotation, bool flip) {
            GameObject corner = InstantiateBorderPart(Corner.gameObject, true);
            corner.transform.Rotate(Vector3.forward, rotation);
            if (flip) {
                var scale = corner.transform.localScale;
                scale.x *= -1;
                corner.transform.localScale = scale;
            }

            return corner;
        }
 
        private void GenerateCorners() {
            // Left top horizontal corner
            GameObject corner = InstantiateCorner(180, true);
            SnapObjectToCanvas(corner, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Right, SA_VertexY.Bottom, new Vector3(m_tilesOffset.x , m_fillerOffset.y));
            // Left top vertical corner


            corner = InstantiateCorner(90, false);
            SnapObjectToCanvas(corner, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Right, SA_VertexY.Bottom, new Vector3(m_fillerOffset.x, m_tilesOffset.y));
            // Right top horizontal corner
            corner = InstantiateCorner(180, false);
            SnapObjectToCanvas(corner, SA_VertexX.Right, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Bottom, new Vector3(-m_tilesOffset.x, m_fillerOffset.y));
            // Right top vertical corner
            corner = InstantiateCorner(270, true);
            SnapObjectToCanvas(corner, SA_VertexX.Right, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Bottom, new Vector3(-m_fillerOffset.x, m_tilesOffset.y));
            // Right bottom horizontal corner
            corner = InstantiateCorner(270, false);
            SnapObjectToCanvas(corner, SA_VertexX.Right, SA_VertexY.Bottom, SA_VertexX.Left, SA_VertexY.Top, new Vector3(-m_fillerOffset.x, -m_tilesOffset.y));
            // Right bottom vertical corner
            corner = InstantiateCorner(0, true);
            SnapObjectToCanvas(corner, SA_VertexX.Right, SA_VertexY.Bottom, SA_VertexX.Left, SA_VertexY.Top, new Vector3(-m_tilesOffset.x, -m_fillerOffset.y ));
            // Left bottom horizontal corner
            corner = InstantiateCorner(0, false);
            SnapObjectToCanvas(corner, SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexX.Right, SA_VertexY.Top, new Vector3(m_tilesOffset.x, -m_fillerOffset.y));
            // Left bottom vertical corner
            corner = InstantiateCorner(90, true);
            SnapObjectToCanvas(corner, SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexX.Right, SA_VertexY.Top, new Vector3(m_fillerOffset.x, -m_tilesOffset.y));
        }


    }
}
