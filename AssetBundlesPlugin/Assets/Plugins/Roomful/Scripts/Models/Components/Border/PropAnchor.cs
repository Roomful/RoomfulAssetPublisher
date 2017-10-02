using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class PropAnchor : MonoBehaviour {


        public GameObject Parent;

        [Header("Anchoring")]

        public Vector3 Anchor = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Offset = Vector3.zero;

        [Header("Size Scale")]

        public bool EnableXScale = false;
        public float XSize = 1f;


        public bool EnableYScale = false;
        public float YSize = 1f;



        void Update() {
            Anchor.x = Mathf.Clamp(Anchor.x, 0f, 1f);
            Anchor.y = Mathf.Clamp(Anchor.y, 0f, 1f);
            Anchor.z = Mathf.Clamp(Anchor.z, 0f, 1f);

            XSize = Mathf.Clamp(XSize, 0f, 1f);
            YSize = Mathf.Clamp(YSize, 0f, 1f);


            if (Parent != null) {
                transform.position = Bounds.center;

                float xPos = Bounds.center.x - Bounds.extents.x + Bounds.size.x * Anchor.x;
                float yPos = Bounds.center.y - Bounds.extents.y + Bounds.size.y * Anchor.y;
                float zPos = Bounds.center.z - Bounds.extents.z + Bounds.size.z * Anchor.z;


                // transform.rotation = Quaternion.identity;
                // transform.RotateAround(Vector3.zero, Vector3.up, XSize);

                transform.position = new Vector3(xPos, yPos, zPos);
                transform.localPosition = transform.localPosition + Offset;


                if (EnableXScale) {
                    var text = GetComponent<RoomfulText>();
                    if(text != null) {
                        text.RectTransform.sizeDelta = new Vector2(Bounds.size.x * XSize, text.RectTransform.sizeDelta.y);
                    }
                }

                if (EnableYScale) {
                    var text = GetComponent<RoomfulText>();
                    if (text != null) {
                        text.RectTransform.sizeDelta = new Vector2(text.RectTransform.sizeDelta.x, Bounds.size.y * YSize) ;
                    }
                }

            }

        }


        public Bounds Bounds {
            get {

                bool hasBounds = false;
                var bounds = new Bounds(Vector3.zero, Vector3.zero);

                if (Parent == null) {
                    return bounds;
                }



                Renderer[] ChildrenRenderer = Parent.GetComponentsInChildren<Renderer>();
                Quaternion oldRotation = Parent.transform.rotation;
                Parent.transform.rotation = Quaternion.identity;

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
                Parent.transform.rotation = oldRotation;


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


    }
}