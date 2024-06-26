﻿using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    class AssetBounds
    {
        public Bounds Calculate(GameObject go) {
            var hasBounds = false;

            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            var childrenRenderer = go.GetComponentsInChildren<Renderer>();

            var oldRotation = go.transform.rotation;
            go.transform.rotation = Quaternion.identity;

            foreach (var child in childrenRenderer) {
                if (child == null) {
                    continue;
                }

                if (!IsValidForBounds(child)) {
                    continue;
                }

                if (!hasBounds) {
                    bounds = child.bounds;
                    hasBounds = true;
                }
                else {
                    bounds.Encapsulate(child.bounds);
                }
            }

            go.transform.rotation = oldRotation;
            return bounds;
        }

        protected virtual bool IsValidForBounds(Renderer renderer) {
            if (renderer.GetComponent<ParticleSystem>() != null) {
                return false;
            }

            if (IsIgnored(renderer.transform)) {
                return false;
            }

            return true;
        }

        bool IsIgnored(Transform go) {
            var testedObject = go;
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