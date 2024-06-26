using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    public static class Scene
    {
        public static void Update() {
            var components = Object.FindObjectsOfType<Component>();
            var propComponents = new List<IPropPublihserComponent>();

            foreach (var component in components) {
                if (component is IPropPublihserComponent propComponent) {
                    if (!IsAnchored(component)) {
                        propComponents.Add(propComponent);
                    }
                }
            }

            propComponents.Sort(new PriorityComparer());

            foreach (var c in propComponents) {
                c.Update();
            }
        }

        private static IAsset s_activeAsset = null;

        internal static IAsset ActiveAsset {
            get {
                if (s_activeAsset == null) {
                    s_activeAsset = FindObjectWithType<IAsset>();
                }

                return s_activeAsset;
            }
        }

        private static T FindObjectWithType<T>() {
            var foundObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var o in foundObjects) {
                var go = (GameObject) o;
                var target = go.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }

            return default;
        }

        private static bool IsAnchored(Component comp) {
            var testedObject = comp.transform;
            while (testedObject != null) {
                var anchor = testedObject.GetComponent<PropAnchor>();
                if (anchor != null) {
                    if (anchor.GetInstanceID() != comp.GetInstanceID()) {
                        return true;
                    }
                }

                testedObject = testedObject.parent;
            }

            return false;
        }

        public static Bounds GetBounds(GameObject go, bool includeIgnoredLayers = false) {
            var hasBounds = false;
            var bounds = new Bounds(Vector3.zero, Vector3.zero);

            var childrenRenderer = go.GetComponentsInChildren<Renderer>();

            foreach (var child in childrenRenderer) {
                if (IsIgnored(child.transform) && !includeIgnoredLayers) {
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

            var textRenderer = go.GetComponentsInChildren<RoomfulText>();
            foreach (var text in textRenderer) {
                if (IsIgnored(text.transform) && !includeIgnoredLayers) {
                    continue;
                }

                var b = new Bounds(text.transform.position, new Vector3(text.Width, text.Height, 0f));
                if (!hasBounds) {
                    bounds = b;
                    hasBounds = true;
                }
                else {
                    bounds.Encapsulate(b);
                }
            }

            return bounds;
        }

        private static bool IsIgnored(Transform go) {
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