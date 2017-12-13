using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{

    public class AssetBounds
    {


        public Bounds Calculate(GameObject go) {

            bool hasBounds = false;

            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            Renderer[] ChildrenRenderer = go.GetComponentsInChildren<Renderer>();

            Quaternion oldRotation = go.transform.rotation;
            go.transform.rotation = Quaternion.identity;

            foreach (Renderer child in ChildrenRenderer) {

                if(child == null) {
                    continue;
                }

                if(!IsValidForBounds(child)) {
                    continue;
                }

                if (!hasBounds) {
                    bounds = child.bounds;
                    hasBounds = true;
                } else {
                    bounds.Encapsulate(child.bounds);
                }
            }

            go.transform.rotation = oldRotation;
            return bounds;
        }


        public virtual bool IsValidForBounds(Renderer renderer) {

            if (renderer.GetComponent<ParticleSystem>() != null) {
                return false;
            }


            if (IsIgnored(renderer.transform)) {
                return false;
            }

            return true;
        }



        private bool IsIgnored(Transform go) {

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