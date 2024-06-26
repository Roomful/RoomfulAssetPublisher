using System.Collections.Generic;
using UnityEngine;
using net.roomful.assets.serialization;
using Scripts.Extensions;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal class PropAnchor : MonoBehaviour, IPropPublihserComponent
    {
        public void Update() {
            Settings.Anchor.x = Mathf.Clamp(Settings.Anchor.x, 0f, 1f);
            Settings.Anchor.y = Mathf.Clamp(Settings.Anchor.y, 0f, 1f);
            Settings.Anchor.z = Mathf.Clamp(Settings.Anchor.z, 0f, 1f);

            Settings.RendererPivot.x = Mathf.Clamp(Settings.RendererPivot.x, 0f, 1f);
            Settings.RendererPivot.y = Mathf.Clamp(Settings.RendererPivot.y, 0f, 1f);
            Settings.RendererPivot.z = Mathf.Clamp(Settings.RendererPivot.z, 0f, 1f);

            Settings.XSize = Mathf.Clamp(Settings.XSize, 0.001f, 1f);
            Settings.YSize = Mathf.Clamp(Settings.YSize, 0.001f, 1f);

            var smartParent = Settings.GetSmartParent();
            if (smartParent != null) {
                var parentBounds = Scene.GetBounds(smartParent);
                transform.position = parentBounds.center;

                var xPos = parentBounds.center.x - parentBounds.extents.x + parentBounds.size.x * Settings.Anchor.x;
                var yPos = parentBounds.center.y - parentBounds.extents.y + parentBounds.size.y * Settings.Anchor.y;
                var zPos = parentBounds.center.z - parentBounds.extents.z + parentBounds.size.z * Settings.Anchor.z;

                transform.position = new Vector3(xPos, yPos, zPos);
                var bounds = Scene.GetBounds(gameObject, true);

                if (Settings.UseRendererPivot) {
                    var x = bounds.center.x - bounds.extents.x + bounds.size.x * Settings.RendererPivot.x;
                    var y = bounds.center.y - bounds.extents.y + bounds.size.y * Settings.RendererPivot.y;
                    var z = bounds.center.z - bounds.extents.z + bounds.size.z * Settings.RendererPivot.z;

                    var anchorPoint = new Vector3(x, y, z);

                    var diff = transform.position - anchorPoint;
                    transform.position = transform.position + diff;
                }

                transform.localPosition = transform.localPosition + (Settings.Offset / CurrentProp.Scale);

                if (Settings.EnableXScale) {
                    var text = GetComponent<RoomfulText>();
                    if (text != null) {
                        var x = parentBounds.size.x * Settings.XSize / text.transform.lossyScale.x;
                        text.RectTransform.sizeDelta = new Vector2(x, text.RectTransform.sizeDelta.y);
                    }
                }

                if (Settings.EnableYScale) {
                    var text = GetComponent<RoomfulText>();
                    if (text != null) {
                        var y = parentBounds.size.y * Settings.YSize / text.transform.lossyScale.y;
                        text.RectTransform.sizeDelta = new Vector2(text.RectTransform.sizeDelta.x, y);
                    }
                }

                UpdateChildComponents();
            }
        }

        private void UpdateChildComponents() {
            var components = transform.GetComponentsInChildren<Component>();
            var propComponents = new List<IPropPublihserComponent>();

            foreach (var component in components) {
                if (component is IPropPublihserComponent && (component.GetInstanceID() != GetInstanceID())) {
                    propComponents.Add(component as IPropPublihserComponent);
                }
            }

            propComponents.Sort(new PriorityComparer());

            foreach (var c in propComponents) {
                c.Update();
            }
        }

        public void PrepareForUpload() {
            DestroyImmediate(this);
        }

        public void RemoveSilhouette() { }

        public PropComponentUpdatePriority UpdatePriority => PropComponentUpdatePriority.Lowest;

        private SerializedAnchor Settings {
            get {
                var settings = GetComponent<SerializedAnchor>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedAnchor>();
                }

                return settings;
            }
        }

        private PropAsset CurrentProp => FindObjectOfType<PropAsset>();
    }
}