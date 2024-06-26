using UnityEngine;
using RF.Room;

// Copyright Roomful 2013-2020. All rights reserved.

namespace RF.AssetBundles
{
    public static class SceneUtility
    {
        public static Bounds GetBounds(GameObject gameObject, bool includeIgnoredLayers, bool resetRotation, bool resetPosition) {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            var oldRotation = gameObject.transform.rotation;
            if (resetRotation) {
                gameObject.transform.rotation = Quaternion.identity;
            }

            var oldPos = gameObject.transform.position;
            if (resetPosition) {
                gameObject.transform.position = Vector3.zero;
            }

            CalculateBounds(ref bounds, gameObject, includeIgnoredLayers);

            gameObject.transform.rotation = oldRotation;
            gameObject.transform.position = oldPos;
            return bounds;
        }

        private static void CalculateBounds(ref Bounds bounds, GameObject gameObject, bool includeIgnoredLayers) {
            var hasBounds = false;
            var childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in childrenRenderers) {
                if (renderer == null || renderer.gameObject == null) {
                    continue;
                }

                if (!includeIgnoredLayers) {
                    if (IsIgnored(renderer.transform)) {
                        continue;
                    }
                }

                if (renderer.GetComponent<ParticleSystem>() != null) {
                    continue;
                }

                if (!hasBounds) {
                    bounds = renderer.bounds;
                    hasBounds = true;
                }
                else {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }

        public static bool IsIgnored(Transform transform) {
            if (transform.tag.Equals(RoomTags.IGNORED_BOUNDS)) {
                return true;
            }

            return false;
        }
    }
}
