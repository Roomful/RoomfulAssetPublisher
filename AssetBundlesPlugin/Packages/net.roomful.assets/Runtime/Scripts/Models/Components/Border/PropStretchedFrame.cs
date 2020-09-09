using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal class PropStretchedFrame : AbstractPropFrame
    {
        public SerializedFrame Settings {
            get {
                var settings = GetComponent<SerializedFrame>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedFrame>();
                }

                settings.hideFlags = HideFlags.None;

                return settings;
            }
        }

        public override void SetBackOffset(float offset) {
            Settings.BackOffset = offset;
        }

        protected override void CheckHierarchy() {
            if (Corner != null) {
                Corner.transform.parent = GetLayer(BorderLayers.BorderParts);
                Corner.gameObject.SetActive(false);
                Corner.gameObject.name = CORNER_NAME;

                if (Corner == Border) {
                    Border = null;
                }
            }

            if (Border != null) {
                Border.transform.parent = GetLayer(BorderLayers.BorderParts);
                Border.gameObject.SetActive(false);
                Border.gameObject.name = BORDER_NAME;
            }

            if (Back != null) {
                Back.transform.parent = GetLayer(BorderLayers.BorderParts);
                Back.gameObject.SetActive(false);
                Back.gameObject.name = BACK_NAME;
            }

            var borderParts = GetLayer(BorderLayers.BorderParts).gameObject;
            var parts = GetLayer(BorderLayers.BorderParts).GetComponentsInChildren<Transform>(true);
            foreach (var part in parts) {
                var go = part.gameObject;
                if (go != Border && go != Corner && go != Back && go != borderParts) {
                    DestroyImmediate(go);
                }
            }
        }

        protected override void GenerateFrame() {
            var oldRotation = transform.rotation;
            transform.rotation = Quaternion.identity;

            var GeneratedBorder = GetLayer(BorderLayers.GeneratedBorder);

            // remove all chields from GeneratedBorder
            var children = new List<GameObject>();
            foreach (Transform child in GeneratedBorder) children.Add(child.gameObject);
            children.ForEach(child => DestroyImmediate(child));

            if (Back != null) {
                var back = InstantiateBorderPart(Back.gameObject);

                var canvasW = Bounds.extents.x;
                var canvasH = Bounds.extents.y;

                var backW = back.GetRendererBounds().extents.x;
                var backH = back.GetRendererBounds().extents.x;

                var scaleX = canvasW / backW;
                var scaleY = canvasH / backH;

                // Vector3 backLocaclScale = new Vector3(back.transform.localScale.x, back.transform.localScale.y, back.transform.localScale.z);
                var newScale = new Vector3(back.transform.localScale.x * scaleX, back.transform.localScale.y * scaleY, back.transform.localScale.z);
                back.transform.localScale = newScale;

                back.transform.position = Bounds.GetVertex(SA_VertexX.Right, SA_VertexY.Top, SA_VertexZ.Back);

                var rendererPoint = back.GetVertex(SA_VertexX.Right, SA_VertexY.Top, SA_VertexZ.Front);
                var diff = back.transform.position - rendererPoint;
                back.transform.position += diff;

                var localPos = back.transform.localPosition;
                localPos.z += (Settings.BackOffset / CurrentProp.Scale);
                back.transform.localPosition = localPos;
            }

            if (Border != null && Corner != null) {
                GenerateCorners();
                GenerateStretchedBorders();
            }

            transform.rotation = oldRotation;
        }

        private void GenerateStretchedBorders() {
            var canvasW = Bounds.extents.x;
            var canvasH = Bounds.extents.y;

            var topBorder = InstantiateBorderPart(Border.gameObject);
            var borderW = topBorder.GetRendererBounds().extents.x;

            var scaleX = canvasW / borderW;
            var scaleY = canvasH / borderW;

            var orogonalScale = new Vector3(topBorder.transform.localScale.x, topBorder.transform.localScale.y, topBorder.transform.localScale.z);
            var xScale = new Vector3(orogonalScale.x * scaleX, orogonalScale.y, orogonalScale.z);
            var yScale = new Vector3(orogonalScale.x * scaleY, orogonalScale.y, orogonalScale.z);

            topBorder.transform.localScale = xScale;
            SnapObjectToCanvas(topBorder, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Bottom, Vector3.zero);

            var bottomBorder = InstantiateBorderPart(Border.gameObject);
            bottomBorder.transform.localScale = xScale;
            bottomBorder.transform.Rotate(Vector3.forward, 180);
            SnapObjectToCanvas(bottomBorder, SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexX.Left, SA_VertexY.Top, Vector3.zero);

            var rightBorder = InstantiateBorderPart(Border.gameObject);
            rightBorder.transform.localScale = yScale;
            rightBorder.transform.Rotate(Vector3.forward, 90);
            SnapObjectToCanvas(rightBorder, SA_VertexX.Right, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Top, Vector3.zero);

            var leftBorder = InstantiateBorderPart(Border.gameObject);
            leftBorder.transform.localScale = yScale;
            leftBorder.transform.Rotate(Vector3.forward, 270);
            SnapObjectToCanvas(leftBorder, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Right, SA_VertexY.Top, Vector3.zero);
        }

        private void GenerateCorners() {
            var leftTopCorner = InstantiateBorderPart(Corner.gameObject);
            SnapObjectToCanvas(leftTopCorner, SA_VertexX.Left, SA_VertexY.Top, SA_VertexX.Right, SA_VertexY.Bottom, Vector3.zero);

            var rightTopCorner = InstantiateBorderPart(Corner.gameObject);
            rightTopCorner.transform.Rotate(Vector3.forward, 90f);
            SnapObjectToCanvas(rightTopCorner, SA_VertexX.Right, SA_VertexY.Top, SA_VertexX.Left, SA_VertexY.Bottom, Vector3.zero);

            var rightBottomCorner = InstantiateBorderPart(Corner.gameObject);
            rightBottomCorner.transform.Rotate(Vector3.forward, 180);
            SnapObjectToCanvas(rightBottomCorner, SA_VertexX.Right, SA_VertexY.Bottom, SA_VertexX.Left, SA_VertexY.Top, Vector3.zero);

            var leftBottomCorner = InstantiateBorderPart(Corner.gameObject);
            leftBottomCorner.transform.Rotate(Vector3.forward, 270);
            SnapObjectToCanvas(leftBottomCorner, SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexX.Right, SA_VertexY.Top, Vector3.zero);
        }

        private static PropAsset CurrentProp => FindObjectOfType<PropAsset>();

        private GameObject InstantiateBorderPart(GameObject reference) {
            var p = Instantiate(reference);
            p.SetActive(true);
            p.transform.parent = GetLayer(BorderLayers.GeneratedBorder);
            p.transform.localScale = reference.transform.localScale / CurrentProp.Scale;
            p.transform.localRotation = Quaternion.identity;
            return p;
        }

        private void SnapObjectToCanvas(GameObject obj, SA_VertexX canvasVertexHorizontal, SA_VertexY canvasVertexVertical, SA_VertexX objectVertexHorizontal, SA_VertexY objectVertexVertical, Vector3 offset) {
            obj.transform.position = Bounds.GetVertex(canvasVertexHorizontal, canvasVertexVertical, SA_VertexZ.Front);

            var rendererPoint = obj.GetVertex(objectVertexHorizontal, objectVertexVertical, SA_VertexZ.Back);
            var diff = obj.transform.position - rendererPoint;
            obj.transform.position += diff;
            obj.transform.position += offset;

            var localPos = obj.transform.localPosition;
            localPos.z += (Settings.FrameOffset / CurrentProp.Scale);
            obj.transform.localPosition = localPos;
        }
    }
}