using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class PropFrame : BaseComponent, IPropComponent
    {

        public GameObject Corner;
        public GameObject Border;
        public GameObject Back;


        private const string CORNER_NAME = "Corner";
        private const string BORDER_NAME = "Border";
        private const string BACK_NAME = "Back";


        void Awake() {

            Transform c = GetLayer(BorderLayers.BorderParts).Find(CORNER_NAME);
            if (c != null) {
                Corner = c.gameObject;
            }

            Transform b = GetLayer(BorderLayers.BorderParts).Find(BORDER_NAME);
            if (b != null) {
                Border = b.gameObject;
            }


            Transform back = GetLayer(BorderLayers.BorderParts).Find(BACK_NAME);
            if (back != null) {
                Back = back.gameObject;
            }

            Update();
        }


		public void OnDestroy() {
			DestroyImmediate (GetLayer (BorderLayers.GeneratedBorder).gameObject);
		}

        public void Update() {
            CheckhHierarchy();
            GenerateFrame();

            if(Prop != null) {
                Prop.Update();
            }
        }


        public void PrepareForUpalod() {

            if (Border != null) {
                Border.SetActive(true);
            }

            if (Corner != null) {
                Corner.SetActive(true);
            }

            if (Back != null) {
                Back.SetActive(true);
            }


            DestroyImmediate(GetLayer(BorderLayers.GeneratedBorder).gameObject);
            DestroyImmediate(this);

        }

        public void GenerateSilhouette() {
            if (Border != null && Corner != null) {
                Transform GeneratedBorder = GetLayer(BorderLayers.GeneratedBorder);
                GameObject borderSilhouette = Instantiate(GeneratedBorder.gameObject) as GameObject;

                borderSilhouette.transform.parent = Silhouette;
                borderSilhouette.Reset();
            }
        }

        public Transform GetLayer(BorderLayers layer) {
            Transform hLayer = transform.Find(layer.ToString());
            if (hLayer == null) {
                GameObject go = new GameObject(layer.ToString());
                hLayer = go.transform;
            }

            hLayer.parent = transform;
            hLayer.localPosition = Vector3.zero;
            hLayer.localRotation = Quaternion.identity;
            hLayer.localScale = Vector3.one;

            return hLayer;
        }



        public Bounds Bounds {
            get {

                bool hasBounds = false;
                var bounds = new Bounds(Vector3.zero, Vector3.zero);


                foreach (BorderLayers layer in System.Enum.GetValues(typeof(BorderLayers))) {
                    GetLayer(layer).gameObject.SetActive(false);
                }


                Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer>();
                Quaternion oldRotation = transform.rotation;
                transform.rotation = Quaternion.identity;

                foreach (Renderer child in ChildrenRenderer) {

                    if (IsIgnored(child.transform)) {
                        continue;
                    }

                    if (!hasBounds) {
                        bounds = child.bounds;
                        hasBounds = true;
                    } else {
                        bounds.Encapsulate(child.bounds);
                    }
                }
                transform.rotation = oldRotation;


                foreach (BorderLayers layer in System.Enum.GetValues(typeof(BorderLayers))) {
                    GetLayer(layer).gameObject.SetActive(true);
                }


                return bounds;
            }


        }

        public bool IsIgnored(Transform go) {

            Transform testedObject = go;
            while (testedObject != null) {
                if (testedObject.GetComponent<SerializedBoundsIgnoreMarker>() != null) {
                    return true;
                }
                testedObject = testedObject.parent;
            }


            return false;
        }



        public SerializedFrame Settings {
            get {

                var settings = GetComponent<SerializedFrame>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedFrame>();
                }

                settings.hideFlags = HideFlags.HideInInspector;

                return settings;
            }
        }


        private void CheckhHierarchy() {
     

            if (Corner != null) {
                Corner.transform.parent = GetLayer(BorderLayers.BorderParts); 
                Corner.gameObject.SetActive(false);
                Corner.gameObject.name = CORNER_NAME;

                if (Corner == Border) { Border = null; }
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

            GameObject borderParts = GetLayer(BorderLayers.BorderParts).gameObject;
            Transform[] parts = GetLayer(BorderLayers.BorderParts).GetComponentsInChildren<Transform>(true);
            foreach(Transform part in parts) {
                GameObject go = part.gameObject;
                if(go != Border && go != Corner && go != Back && go != borderParts) {
                    DestroyImmediate(go);
                }
            }


        }




        private void GenerateFrame() {

            Quaternion oldRotation = transform.rotation;
            transform.rotation = Quaternion.identity;

            Transform GeneratedBorder = GetLayer(BorderLayers.GeneratedBorder);

            // remove all chields from GeneratedBorder
            var children = new List<GameObject>();
            foreach (Transform child in GeneratedBorder) children.Add(child.gameObject);
            children.ForEach(child => DestroyImmediate(child));

            if (Back != null) {
                GameObject back = InstantiateBorderPart(Back.gameObject);


                float canvasW = Bounds.extents.x;
                float canvasH = Bounds.extents.y;

                float backW = back.GetRendererBounds().extents.x;
                float backH = back.GetRendererBounds().extents.x;


                float scaleX = canvasW / backW;
                float scaleY = canvasH / backH;

               // Vector3 backLocaclScale = new Vector3(back.transform.localScale.x, back.transform.localScale.y, back.transform.localScale.z);
                Vector3 newScale = new Vector3(back.transform.localScale.x * scaleX, back.transform.localScale.y * scaleY, back.transform.localScale.z);
                back.transform.localScale = newScale;


                back.transform.position = Bounds.GetVertex(VertexX.Right, VertexY.Top, VertexZ.Back);

                Vector3 rendererPoint = back.GetVertex(VertexX.Right, VertexY.Top, VertexZ.Front);
                Vector3 diff = back.transform.position - rendererPoint;
               // diff.z += Settings.BackOffset;
                back.transform.position += diff;

                Vector3 localPos = back.transform.localPosition;
                localPos.z += Settings.BackOffset;
                back.transform.localPosition = localPos;

            }


            if (Border != null && Corner != null) {

                GameObject corner_left_top = InstantiateBorderPart(Corner.gameObject);
                PutObjectAt(corner_left_top, VertexX.Left, VertexY.Top, VertexX.Right, VertexY.Bottom);


                GameObject corner_right_top = InstantiateBorderPart(Corner.gameObject);
                corner_right_top.transform.Rotate(Vector3.forward, 90f);
                PutObjectAt(corner_right_top, VertexX.Right, VertexY.Top, VertexX.Left, VertexY.Bottom);



                GameObject corner_right_bottom = InstantiateBorderPart(Corner.gameObject);
                corner_right_bottom.transform.Rotate(Vector3.forward, 180);
                PutObjectAt(corner_right_bottom, VertexX.Right, VertexY.Bottom, VertexX.Left, VertexY.Top);

                GameObject corner_left_bottom = InstantiateBorderPart(Corner.gameObject);
                corner_left_bottom.transform.Rotate(Vector3.forward, 270);
                PutObjectAt(corner_left_bottom, VertexX.Left, VertexY.Bottom, VertexX.Right, VertexY.Top);


                GameObject border_top = InstantiateBorderPart(Border.gameObject);


                float canvasW = Bounds.extents.x;
                float borderW = border_top.GetRendererBounds().extents.x;

                float canvasH = Bounds.extents.y;



                float scaleX = canvasW / borderW;
                float scaleY = canvasH / borderW;

                Vector3 orogonalScale = new Vector3(border_top.transform.localScale.x, border_top.transform.localScale.y, border_top.transform.localScale.z);
                Vector3 xScale = new Vector3(orogonalScale.x * scaleX, orogonalScale.y, orogonalScale.z);
                Vector3 yScale = new Vector3(orogonalScale.x * scaleY, orogonalScale.y, orogonalScale.z);


                border_top.transform.localScale = xScale;
                PutObjectAt(border_top, VertexX.Left, VertexY.Top, VertexX.Left, VertexY.Bottom);


                GameObject border_bottom = InstantiateBorderPart(Border.gameObject);
                border_bottom.transform.localScale = xScale;
                border_bottom.transform.Rotate(Vector3.forward, 180);
                PutObjectAt(border_bottom, VertexX.Left, VertexY.Bottom, VertexX.Left, VertexY.Top);



                GameObject border_right = InstantiateBorderPart(Border.gameObject);
                border_right.transform.localScale = yScale;
                border_right.transform.Rotate(Vector3.forward, 90);
                PutObjectAt(border_right, VertexX.Right, VertexY.Top, VertexX.Left, VertexY.Top);



                GameObject border_left = InstantiateBorderPart(Border.gameObject);
                border_left.transform.localScale = yScale;
                border_left.transform.Rotate(Vector3.forward, 270);
                PutObjectAt(border_left, VertexX.Left, VertexY.Top, VertexX.Right, VertexY.Top);
            }


            transform.rotation = oldRotation;
        }


        private GameObject InstantiateBorderPart(GameObject reference) {
            GameObject p = Instantiate(reference) as GameObject;
            p.SetActive(true);
            p.transform.parent = GetLayer(BorderLayers.GeneratedBorder);
            p.transform.localScale = reference.transform.localScale;
            return p;
        }

        private void PutObjectAt(GameObject obj, VertexX CanvasVertexX, VertexY CanvasVertexY, VertexX ObjectVertexX, VertexY ObjectVertexY) {
            obj.transform.position = Bounds.GetVertex(CanvasVertexX, CanvasVertexY, VertexZ.Front);

            Vector3 rendererPoint = obj.GetVertex(ObjectVertexX, ObjectVertexY, VertexZ.Back);
            Vector3 diff = obj.transform.position - rendererPoint;
            obj.transform.position += diff;


            Vector3 localPos = obj.transform.localPosition;
            localPos.z += Settings.FrameOffset;
            obj.transform.localPosition = localPos;

        }

    }
}